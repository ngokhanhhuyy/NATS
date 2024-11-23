namespace NATS.Services;

public class BusinessServiceService : IBusinessServiceService
{
    private readonly DatabaseContext _context;
    private readonly IValidator<BusinessServiceRequestDto> _validator;
    private readonly IPhotoService _photoService;

    public BusinessServiceService(
            DatabaseContext context,
            IValidator<BusinessServiceRequestDto> validator,
            IPhotoService photoService)
    {
        _context = context;
        _validator = validator;
        _photoService = photoService;
    }

    /// <summary>
    /// Get a list of business services with basic information and thumbnail only.
    /// </summary>
    /// <returns>
    /// ServiceResult object that contains the list of results.
    /// </returns>
    public async Task<ServiceResult<List<BusinessServiceBasicResponseDto>>> GetBasicListAsync()
    {
        List<BusinessServiceBasicResponseDto> responseDtos;
        responseDtos = await _context.BusinessServices
            .OrderBy(bs => bs.Id)
            .Select(bs => new BusinessServiceBasicResponseDto
            {
                Id = bs.Id,
                Name = bs.Name,
                Summary = bs.Summary,
                ThumbnailUrl = bs.ThumbnailUrl
            }).ToListAsync();
        return ServiceResult<List<BusinessServiceBasicResponseDto>>.Success(responseDtos);
    }

    /// <summary>
    /// Get a list of business services with detail information,
    /// containing the thumbnail url, all features and photos.
    /// </summary>
    /// <returns>
    /// ServiceResult object that contains the list of results.
    /// </returns>
    public async Task<ServiceResult<List<BusinessServiceDetailResponseDto>>> GetDetailListAsync()
    {
        // Fetch the list of detail business service from the database
        List<BusinessServiceDetailResponseDto> responseDtos;
        responseDtos = await _context.BusinessServices
            .Include(c => c.Features)
            .Include(c => c.Photos)
            .OrderBy(bs => bs.Id)
            .Select(bs => new BusinessServiceDetailResponseDto
            {
                Id = bs.Id,
                Name = bs.Name,
                Summary = bs.Summary,
                Detail = bs.Detail,
                ThumbnailUrl = bs.ThumbnailUrl,
                Features = bs.Features
                    .OrderBy(bsf => bsf.Id)
                    .Select(bsf => new BusinessServiceFeatureResponseDto
                    {
                        Id = bsf.Id,
                        Content = bsf.Content
                    }).ToList(),
                Photos = bs.Photos
                    .OrderBy(bsp => bsp.Id)
                    .Select(bsp => new BusinessServicePhotoResponseDto
                    {
                        Id = bsp.Id,
                        Url = bsp.Url
                    }).ToList()
            }).ToListAsync();
        return ServiceResult<List<BusinessServiceDetailResponseDto>>.Success(responseDtos);
    }

    /// <summary>
    /// Get business service by given id with detail information,
    /// containing the thumbnail url, all features and photos.
    /// </summary>
    /// <param name="id">
    /// The id of the business service
    /// </param>
    /// <returns>
    /// ServiceResult object that contain the data of the business service if the business service with given id
    /// exists. Otherwise, a ServiceResult object which contains NotFound error will be returned.
    /// </returns>
    public async Task<ServiceResult<BusinessServiceDetailResponseDto>> GetAsync(int id)
    {
        // Fetch the entity with given id from the database
        BusinessService service = await _context.BusinessServices
            .Include(bs => bs.Features)
            .Include(bs => bs.Photos)
            .SingleOrDefaultAsync(bs => bs.Id == id);
        
        // Ensure the entity exists in the database
        if (service == null)
        {
            return ServiceResult<BusinessServiceDetailResponseDto>.Failed(
                ServiceError.NotFoundByProperty(
                    nameof(BusinessService),
                    nameof(id),
                    id.ToString()));
        }

        // Map entity properties to response dto
        BusinessServiceDetailResponseDto responseDto;
        responseDto = new BusinessServiceDetailResponseDto
        {
            Id = service.Id,
            Name = service.Name,
            Summary = service.Summary,
            Detail = service.Detail,
            ThumbnailUrl = service.ThumbnailUrl,
            Features = service.Features
                .OrderBy(bsf => bsf.Id)
                .Select(bsf => new BusinessServiceFeatureResponseDto
                {
                    Id = bsf.Id,
                    Content = bsf.Content
                }).ToList(),
            Photos = service.Photos
                .OrderBy(bsp => bsp.Id)
                .Select(bsp => new BusinessServicePhotoResponseDto
                {
                    Id = bsp.Id,
                    Url = bsp.Url
                }).ToList()
        };

        // Return the response dto
        return ServiceResult<BusinessServiceDetailResponseDto>.Success(responseDto);
    }

    /// <summary>
    /// Create a business service by the data sent from the request, containing thumbnail file, features and photos.
    /// </summary>
    /// <param name="requestDto">
    /// An object that contains all data for a new business service sent from request.
    /// </param>
    /// <returns>
    /// ServiceResult object that contain the data of the created business service if the operation was successful.
    /// Otherwise, a ServiceResult object which contains Validation, NotFound or Operation error will be returned.
    /// </returns>
    public async Task<ServiceResult<BusinessServiceDetailResponseDto>> CreateAsync(
            BusinessServiceRequestDto requestDto)
    {
        // Validate data from request
        ValidationResult result = _validator.Validate(
            requestDto.TransformValues(),
            options => options.IncludeRuleSets("Create")
                .IncludeRulesNotInRuleSet());
        if (!result.IsValid)
        {
            return ServiceResult<BusinessServiceDetailResponseDto>.Failed(result.Errors);
        }

        BusinessService businessService = new BusinessService
        {
            Name = requestDto.Name,
            Summary = requestDto.Summary,
            Detail = requestDto.Detail,
            Features = new List<BusinessServiceFeature>(),
            Photos = new List<BusinessServicePhoto>()
        };

        // Create new thumbnail if the request contains it
        if (requestDto.ThumbnailFile != null)
        {
            ServiceResult<string> photoServiceResult;
            photoServiceResult = await _photoService.CreateAsync(
                requestDto.ThumbnailFile,
                "services",
                true);
            businessService.ThumbnailUrl = photoServiceResult.ResponseDto;
        }

        // Create features
        if (requestDto.Features != null)
        {
            foreach (BusinessServiceFeatureRequestDto featureRequestDto in requestDto.Features)
            {
                BusinessServiceFeature section = new BusinessServiceFeature
                {
                    Content = featureRequestDto.Content
                };
                businessService.Features.Add(section);
            }
        }

        // Create photos
        if (requestDto.Photos != null)
        {
            foreach (BusinessServicePhotoRequestDto photoRequestDto in requestDto.Photos)
            {
                ServiceResult<string> photoServiceResult;
                photoServiceResult = await _photoService.CreateAsync(
                    photoRequestDto.File,
                    "services",
                    false);
                BusinessServicePhoto photo = new BusinessServicePhoto
                {
                    Url = photoServiceResult.ResponseDto
                };
                businessService.Photos.Add(photo);
            }
        }

        // Save changes
        _context.BusinessServices.Add(businessService);
        await _context.SaveChangesAsync();

        // Return the data of the created entity
        BusinessServiceDetailResponseDto responseDto = new BusinessServiceDetailResponseDto
        {
            Id = businessService.Id,
            Name = businessService.Name,
            Summary = businessService.Summary,
            Detail = businessService.Detail,
            ThumbnailUrl = businessService.ThumbnailUrl,
            Features = businessService.Features
                .OrderBy(feature => feature.Id)
                .Select(feature => new BusinessServiceFeatureResponseDto
                {
                    Id = feature.Id,
                    Content = feature.Content
                }).ToList(),
            Photos = businessService.Photos
                .OrderBy(photo => photo.Id)
                .Select(photo => new BusinessServicePhotoResponseDto
                {
                    Id = photo.Id,
                    Url = photo.Url
                }).ToList()
        };
        return ServiceResult<BusinessServiceDetailResponseDto>.Success(responseDto);
    }

    /// <summary>
    /// Update the business service which has the given id with the data sent from the request,
    /// containing thumbnail file, features and photos.
    /// </summary>
    /// <param name="id">
    /// The id of the business service to be updated
    /// </param>
    /// <param name="requestDto">
    /// An object that contains all data for a new business service sent from request.
    /// </param>
    /// <returns>
    /// ServiceResult object that contain the data of the updated business service if the operation was successful.
    /// Otherwise, a ServiceResult object which contains Validation, NotFound or Operation error will be returned.
    /// </returns>
    public async Task<ServiceResult<BusinessServiceDetailResponseDto>> UpdateAsync(
            int id,
            BusinessServiceRequestDto requestDto)
    {
        // Validate data from request
        ValidationResult result = _validator.Validate(
            requestDto.TransformValues(),
            options => options.IncludeRuleSets("Update")
                .IncludeRulesNotInRuleSet());
        if (!result.IsValid)
        {
            return ServiceResult<BusinessServiceDetailResponseDto>.Failed(result.Errors);
        }

        // Fetch the entity in the database
        BusinessService businessService = await _context.BusinessServices
            .Include(bs => bs.Features)
            .Include(bs => bs.Photos)
            .SingleOrDefaultAsync(bs => bs.Id == id);

        // Ensure the entity with given id exists in the database
        if (businessService == null)
        {
            return ServiceResult<BusinessServiceDetailResponseDto>.Failed(
                ServiceError.NotFoundByProperty(
                    nameof(Course),
                    nameof(id),
                    id.ToString()));
        }

        // Replace the thumbnail with a new one if it has been changed
        if (requestDto.ThumbnailChanged)
        {
            // Delete the old thumbnail with the URL stored in the entity property if exists
            if (businessService.ThumbnailUrl != null)
            {
                _photoService.Delete(businessService.ThumbnailUrl);
                businessService.ThumbnailUrl = null;
            }
            // Add a new thumbnail if the request contains it
            if (requestDto.ThumbnailFile != null)
            {
                ServiceResult<string> photoServiceResult = await _photoService.CreateAsync(
                    requestDto.ThumbnailFile,
                    "courses",
                    true);
                businessService.ThumbnailUrl = photoServiceResult.ResponseDto;
            }
        }

        businessService.Name = requestDto.Name;
        businessService.Summary = requestDto.Summary;
        businessService.Detail = requestDto.Detail;

        // Update features
        if (requestDto.Features != null)
        {
            foreach (BusinessServiceFeatureRequestDto featureRequestDto in requestDto.Features)
            {
                // Perform updating operation if this feature has id value.
                // Otherwise, perform creating operation.
                BusinessServiceFeature feature;
                if (featureRequestDto.Id.HasValue)
                {
                    feature = businessService.Features
                        .SingleOrDefault(s => s.Id == featureRequestDto.Id);
                    // Ensure the entity with given id exists.
                    if (feature == null)
                    {
                        return ServiceResult<BusinessServiceDetailResponseDto>.Failed(
                            ServiceError.Incorrect(nameof(featureRequestDto.Id)));
                    }

                    // Perform delete operation if indicated. Otherwise, update it's content.
                    if (featureRequestDto.IsDeleted)
                    {
                        _context.BusinessServiceFeatures.Remove(feature);
                    }
                    else
                    {
                        feature.Content = featureRequestDto.Content;
                    }
                }
                else
                {
                    feature = new BusinessServiceFeature
                    {
                        Content = featureRequestDto.Content,
                        BusinessServiceId = businessService.Id
                    };
                    _context.BusinessServiceFeatures.Add(feature);
                }
            }
        }
        
        // Update photos
        if (requestDto.Photos != null)
        {
            foreach (BusinessServicePhotoRequestDto photoRequestDto in requestDto.Photos)
            {
                BusinessServicePhoto photo;
                ServiceResult<string> photoServiceResult;
                // Perform updating operation if this photo has id value.
                if (photoRequestDto.Id.HasValue)
                {
                    photo = businessService.Photos
                        .SingleOrDefault(p => p.Id == photoRequestDto.Id);

                    // Ensure the photo with given id exists
                    if (photo == null)
                    {
                        return ServiceResult<BusinessServiceDetailResponseDto>.Failed(
                            ServiceError.Incorrect(nameof(photoRequestDto.Id)));
                    }

                    // Delete the old photo having URL stored in the entity property
                    if (photoRequestDto.IsDeleted)
                    {
                        // Delete old photo in filesystem and the entity from the database
                        _photoService.Delete(photo.Url);
                        _context.BusinessServicePhotos.Remove(photo);
                    }
                }
                // Otherwise, perform creating operation.
                else if (!photoRequestDto.IsDeleted)
                {
                    photoServiceResult = await _photoService.CreateAsync(
                        photoRequestDto.File,
                        "courses",
                        false);
                    photo = new BusinessServicePhoto
                    {
                        Url = photoServiceResult.ResponseDto,
                        BusinessServiceId = businessService.Id
                    };
                    _context.BusinessServicePhotos.Add(photo);
                }
            }
        }

        // Save changes
        await _context.SaveChangesAsync();

        // Return the data of the updated entity
        BusinessServiceDetailResponseDto responseDto = new BusinessServiceDetailResponseDto
        {
            Id = businessService.Id,
            Name = businessService.Name,
            Summary = businessService.Summary,
            Detail = businessService.Detail,
            ThumbnailUrl = businessService.ThumbnailUrl,
            Features = businessService.Features
                .OrderBy(feature => feature.Id)
                .Select(feature => new BusinessServiceFeatureResponseDto
                {
                    Id = feature.Id,
                    Content = feature.Content
                }).ToList(),
            Photos = businessService.Photos
                .OrderBy(photo => photo.Id)
                .Select(p => new BusinessServicePhotoResponseDto
                {
                    Id = p.Id,
                    Url = p.Url
                }).ToList()
        };
        return ServiceResult<BusinessServiceDetailResponseDto>.Success(responseDto);
    }

    /// <summary>
    /// Delete a business service which has the given id, including its thumbnail, features and photos.
    /// </summary>
    /// <param name="id">
    /// The id of the business service to be deleted
    /// </param>
    /// <returns>
    /// The id of the business service which has been deleted if the operation was successful.
    /// Otherwise, a ServiceResult object which contains NotFound error.
    /// </returns>
    public async Task<ServiceResult<int>> DeleteAsync(int id)
    {
        // Fetch the entity with given id from the database
        BusinessService businessService = await _context.BusinessServices
            .Include(bs => bs.Features)
            .Include(bs => bs.Photos)
            .SingleOrDefaultAsync(bs => bs.Id == id);

        // Ensure the entity with given id exists in the database
        if (businessService == null)
        {
            return ServiceResult<int>.Failed(ServiceError.NotFoundByProperty(
                nameof(Course),
                nameof(id),
                id.ToString()
            ));
        }

        // Delete all sections
        foreach (BusinessServiceFeature feature in businessService.Features)
        {
            _context.BusinessServiceFeatures.Remove(feature);
        }

        // Delete all photos
        foreach (BusinessServicePhoto photo in businessService.Photos)
        {
            ServiceResult<string> photoServiceResult;
            photoServiceResult = _photoService.Delete(photo.Url);
            _context.BusinessServicePhotos.Remove(photo);
        }

        // Delete the entity
        _context.BusinessServices.Remove(businessService);

        // Save changes
        await _context.SaveChangesAsync();

        // Return the id of the deleted entity
        return ServiceResult<int>.Success(businessService.Id);
    }
}