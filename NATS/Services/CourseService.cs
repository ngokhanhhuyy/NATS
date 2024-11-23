namespace NATS.Services;

public class CourseService : ICourseService
{
    private readonly DatabaseContext _context;
    private readonly IValidator<CourseRequestDto> _validator;
    private readonly IPhotoService _photoService;

    public CourseService(
            DatabaseContext context,
            IValidator<CourseRequestDto> validator,
            IPhotoService photoService)
    {
        _context = context;
        _validator = validator;
        _photoService = photoService;
    }

    /// <summary>
    /// Get a list of courses with basic information and thumbnail only.
    /// </summary>
    /// <returns>
    /// ServiceResult object that contains the list of results.
    /// </returns>
    public async Task<ServiceResult<List<CourseBasicResponseDto>>> GetBasicListAsync()
    {
        List<CourseBasicResponseDto> responseDtos = await _context.Courses
            .OrderBy(c => c.Id)
            .Select(c => new CourseBasicResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Summary = c.Summary,
                ThumbnailUrl = c.ThumbnailUrl
            }).ToListAsync();
        return ServiceResult<List<CourseBasicResponseDto>>.Success(responseDtos);
    }

    /// <summary>
    /// Get a list of courses with detail information,
    /// containing the thumbnail url, all sections and photos.
    /// </summary>
    /// <returns>
    /// ServiceResult object that contains the list of results.
    /// </returns>
    public async Task<ServiceResult<List<CourseDetailResponseDto>>> GetDetailListAsync()
    {
        // Fetch a list of detail entities in the database
        List<CourseDetailResponseDto> responseDtos = await _context.Courses
            .Include(c => c.Sections)
            .Include(c => c.Photos)
            .OrderBy(c => c.Id)
            .Select(c => new CourseDetailResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Summary = c.Summary,
                Detail = c.Detail,
                ThumbnailUrl = c.ThumbnailUrl,
                Sections = c.Sections
                    .OrderBy(cs => cs.Id)
                    .Select(cs => new CourseSectionResponseDto
                    {
                        Id = cs.Id,
                        Content = cs.Content
                    }).ToList(),
                Photos = c.Photos
                    .OrderBy(cp => cp.Id)
                    .Select(cp => new CoursePhotoResponseDto
                    {
                        Id = cp.Id,
                        Url = cp.Url
                    }).ToList()
            })
            .ToListAsync();
        return ServiceResult<List<CourseDetailResponseDto>>.Success(responseDtos);
    }

    /// <summary>
    /// Get course by given id with detail information,
    /// containing the thumbnail url, all sections and photos.
    /// </summary>
    /// <param name="id">
    /// The id of the course.
    /// </param>
    /// <returns>
    /// ServiceResult object that contain the data of the course if the course with given id exists.
    /// Otherwise, a ServiceResult object which contains NotFound error will be returned.
    /// </returns>
    public async Task<ServiceResult<CourseDetailResponseDto>> GetAsync(int id)
    {
        // Fetch the entity with given id from the database
        Course course = await _context.Courses
            .Include(c => c.Sections)
            .Include(c => c.Photos)
            .SingleOrDefaultAsync(c => c.Id == id);

        // Ensure the entity exists
        if (course == null)
        {
            return ServiceResult<CourseDetailResponseDto>.Failed(ServiceError.NotFoundByProperty(
                nameof(Course),
                nameof(id),
                id.ToString()));
        }

        // Return the data of the entity
        CourseDetailResponseDto responseDto = new CourseDetailResponseDto
        {
            Id = course.Id,
            Name = course.Name,
            Summary = course.Summary,
            Detail = course.Detail,
            ThumbnailUrl = course.ThumbnailUrl,
            Sections = course.Sections
                .OrderBy(section => section.Id)
                .Select(cs => new CourseSectionResponseDto
                {
                    Id = cs.Id,
                    Content = cs.Content
                }).ToList(),
            Photos = course.Photos
                .OrderBy(photo => photo.Id)
                .Select(cp => new CoursePhotoResponseDto
                {
                    Id = cp.Id,
                    Url = cp.Url
                }).ToList()
        };
        return ServiceResult<CourseDetailResponseDto>.Success(responseDto);
    }

    /// <summary>
    /// Create a course by the data sent from the request, containing thumbnail file, sections and photos.
    /// </summary>
    /// <param name="requestDto">
    /// An object that contains all data for a new course sent from request.
    /// </param>
    /// <returns>
    /// ServiceResult object that contain the data of the created course if the operation was successful.
    /// Otherwise, a ServiceResult object which contains Validation, NotFound or Operation error will be returned.
    /// </returns>
    public async Task<ServiceResult<CourseDetailResponseDto>> CreateAsync(CourseRequestDto requestDto)
    {
        // Validate data from request
        ValidationResult result = _validator.Validate(
            requestDto.TransformValues(),
            options => options.IncludeRuleSets("Create")
                .IncludeRulesNotInRuleSet());
        if (!result.IsValid)
        {
            return ServiceResult<CourseDetailResponseDto>.Failed(result.Errors);
        }

        Course course = new Course
        {
            Name = requestDto.Name,
            Summary = requestDto.Summary,
            Detail = requestDto.Detail,
            Sections = new List<CourseSection>(),
            Photos = new List<CoursePhoto>()
        };

        // Create new thumbnail if the request contains it
        if (requestDto.ThumbnailFile != null)
        {
            ServiceResult<string> photoServiceResult;
            photoServiceResult = await _photoService.CreateAsync(
                requestDto.ThumbnailFile,
                "courses",
                true);
            course.ThumbnailUrl = photoServiceResult.ResponseDto;
        }

        // Create sections
        if (requestDto.Sections != null)
        {
            foreach (CourseSectionRequestDto sectionRequestDto in requestDto.Sections)
            {
                CourseSection section = new CourseSection
                {
                    Content = sectionRequestDto.Content
                };
                course.Sections.Add(section);
            }
        }

        // Create photos
        if (requestDto.Photos != null)
        {
            foreach (CoursePhotoRequestDto photoRequestDto in requestDto.Photos)
            {
                ServiceResult<string> photoServiceResult;
                photoServiceResult = await _photoService.CreateAsync(
                    photoRequestDto.File,
                    "courses",
                    false);
                CoursePhoto photo = new CoursePhoto
                {
                    Url = photoServiceResult.ResponseDto
                };
                course.Photos.Add(photo);
            }
        }

        // Save changes
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        // Return the data of the created entity
        CourseDetailResponseDto responseDto = new CourseDetailResponseDto
        {
            Id = course.Id,
            Name = course.Name,
            Summary = course.Summary,
            Detail = course.Detail,
            ThumbnailUrl = course.ThumbnailUrl,
            Sections = course.Sections
                .OrderBy(section => section.Id)
                .Select(section => new CourseSectionResponseDto
                {
                    Id = section.Id,
                    Content = section.Content
                }).ToList(),
            Photos = course.Photos
                .OrderBy(photo => photo.Id)
                .Select(photo => new CoursePhotoResponseDto
                {
                    Id = photo.Id,
                    Url = photo.Url
                }).ToList()
        };
        return ServiceResult<CourseDetailResponseDto>.Success(responseDto);
    }

    /// <summary>
    /// Update the course which has the given id with the data sent from the request,
    /// containing thumbnail file, sections and photos.
    /// </summary>
    /// <param name="id">
    /// The id of the course to be deleted.
    /// </param>
    /// <param name="requestDto">
    /// An object that contains all data for a new course sent from request.
    /// </param>
    /// <returns>
    /// ServiceResult object that contain the data of the updated course if the operation was successful.
    /// Otherwise, a ServiceResult object which contains Validation, NotFound or Operation error will be returned.
    /// </returns>
    public async Task<ServiceResult<CourseDetailResponseDto>> UpdateAsync(
            int id,
            CourseRequestDto requestDto)
    {
        // Validate data from request
        ValidationResult result = _validator.Validate(
            requestDto.TransformValues(),
            options => options.IncludeRuleSets("Update")
                .IncludeRulesNotInRuleSet());
        if (!result.IsValid)
        {
            return ServiceResult<CourseDetailResponseDto>.Failed(result.Errors);
        }

        // Fetch the entity in the database
        Course course = await _context.Courses
            .Include(c => c.Sections)
            .Include(c => c.Photos)
            .SingleOrDefaultAsync(c => c.Id == id);

        // Ensure the entity with given id exists in the database
        if (course == null)
        {
            return ServiceResult<CourseDetailResponseDto>.Failed(
                ServiceError.NotFoundByProperty(
                    nameof(Course),
                    nameof(id),
                    id.ToString()));
        }

        // Replace the thumbnail with a new one if it has been changed
        if (requestDto.ThumbnailChanged)
        {
            // Delete the old thumbnail with the URL stored in the entity property if exists
            if (course.ThumbnailUrl != null)
            {
                _photoService.Delete(course.ThumbnailUrl);
                course.ThumbnailUrl = null;
            }
            // Add a new thumbnail if the request contains it
            if (requestDto.ThumbnailFile != null)
            {
                ServiceResult<string> photoServiceResult = await _photoService.CreateAsync(
                    requestDto.ThumbnailFile,
                    "courses",
                    true);
                course.ThumbnailUrl = photoServiceResult.ResponseDto;
            }
        }

        course.Name = requestDto.Name;
        course.Summary = requestDto.Summary;
        course.Detail = requestDto.Detail;

        // Update sections
        if (requestDto.Sections != null)
        {
            foreach (CourseSectionRequestDto sectionRequestDto in requestDto.Sections)
            {
                // Perform updating operation if this section has id value.
                // Otherwise, perform creating operation.
                CourseSection section;
                if (sectionRequestDto.Id.HasValue)
                {
                    section = course.Sections
                        .SingleOrDefault(s => s.Id == sectionRequestDto.Id);
                    // Ensure the entity with given id exists.
                    if (section == null)
                    {
                        return ServiceResult<CourseDetailResponseDto>.Failed(
                            ServiceError.Incorrect(nameof(sectionRequestDto.Id)));
                    }

                    // Perform delete operation if indicated. Otherwise, update it's content.
                    if (sectionRequestDto.IsDeleted)
                    {
                        _context.CourseSections.Remove(section);
                    }
                    else
                    {
                        section.Content = sectionRequestDto.Content;
                    }
                }
                else
                {
                    section = new CourseSection
                    {
                        Content = sectionRequestDto.Content,
                        CourseId = course.Id
                    };
                    _context.CourseSections.Add(section);
                }
            }
        }

        // Update photos
        if (requestDto.Photos != null)
        {
            foreach (CoursePhotoRequestDto photoRequestDto in requestDto.Photos)
            {
                CoursePhoto photo;
                ServiceResult<string> photoServiceResult;
                // Perform updating operation if this photo has id value.
                if (photoRequestDto.Id.HasValue)
                {
                    photo = course.Photos
                        .SingleOrDefault(p => p.Id == photoRequestDto.Id);

                    // Ensure the photo with given id exists
                    if (photo == null)
                    {
                        return ServiceResult<CourseDetailResponseDto>.Failed(
                            ServiceError.Incorrect(nameof(photoRequestDto.Id)));
                    }

                    // Delete the old photo having URL stored in the entity property
                    if (photoRequestDto.IsDeleted)
                    {
                        // Delete old photo in filesystem and the entity from the database
                        _photoService.Delete(photo.Url);
                        _context.CoursePhotos.Remove(photo);
                    }
                }
                // Otherwise, perform creating operation.
                else if (!photoRequestDto.IsDeleted)
                {
                    photoServiceResult = await _photoService.CreateAsync(
                        photoRequestDto.File,
                        "courses",
                        false);
                    photo = new CoursePhoto
                    {
                        Url = photoServiceResult.ResponseDto,
                        CourseId = course.Id
                    };
                    _context.CoursePhotos.Add(photo);
                }
            }
        }

        // Save changes
        await _context.SaveChangesAsync();

        // Return the data of the updated entity
        CourseDetailResponseDto responseDto = new CourseDetailResponseDto
        {
            Id = course.Id,
            Name = course.Name,
            Summary = course.Summary,
            Detail = course.Detail,
            ThumbnailUrl = course.ThumbnailUrl,
            Sections = course.Sections
                .OrderBy(section => section.Id)
                .Select(s => new CourseSectionResponseDto
                {
                    Id = s.Id,
                    Content = s.Content
                }).ToList(),
            Photos = course.Photos
                .OrderBy(photo => photo.Id)
                .Select(p => new CoursePhotoResponseDto
                {
                    Id = p.Id,
                    Url = p.Url
                }).ToList()
        };
        return ServiceResult<CourseDetailResponseDto>.Success(responseDto);
    }

    /// <summary>
    /// Delete a course which has the given id, including its thumbnail, sections and photos.
    /// </summary>
    /// <param name="id">
    /// The id of the course to be deleted
    /// </param>
    /// <returns>
    /// The id of the course which has been deleted if the operation was successful.
    /// Otherwise, a ServiceResult object which contains NotFound error.
    /// </returns>
    public async Task<ServiceResult<int>> DeleteAsync(int id)
    {
        // Fetch the entity with given id from the database
        Course course = await _context.Courses
            .Include(c => c.Sections)
            .Include(c => c.Photos)
            .SingleOrDefaultAsync(c => c.Id == id);

        // Ensure the entity with given id exists in the database
        if (course == null)
        {
            return ServiceResult<int>.Failed(ServiceError.NotFoundByProperty(
                nameof(Course),
                nameof(id),
                id.ToString()
            ));
        }

        // Delete all sections
        foreach (CourseSection section in course.Sections)
        {
            _context.CourseSections.Remove(section);
        }

        // Delete all photos
        foreach (CoursePhoto photo in course.Photos)
        {
            ServiceResult<string> photoServiceResult;
            photoServiceResult = _photoService.Delete(photo.Url);
            _context.CoursePhotos.Remove(photo);
        }

        // Delete the entity
        _context.Courses.Remove(course);

        // Save changes
        await _context.SaveChangesAsync();

        // Return the id of the deleted entity
        return ServiceResult<int>.Success(course.Id);
    }
}
