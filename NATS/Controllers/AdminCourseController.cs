namespace NATS.Controllers;

[Route("quan-tri/noi-dung/khoa-hoc")]
[Authorize]
public class AdminCourseController : Controller
{
    private readonly ICourseService _courseService;

    public AdminCourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        ServiceResult <List<CourseBasicResponseDto>> serviceResult;
        serviceResult = await _courseService.GetBasicListAsync();

        CourseBasicListViewModel model = new CourseBasicListViewModel
        {
            Items = serviceResult.ResponseDto
                .Select(c => new CourseBasicViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Summary = c.Name,
                    ThumbnailUrl = c.ThumbnailUrl
                }).ToList()
        };
        return View("~/Views/Admin/Course/CourseList.cshtml", model);
    }

    [HttpGet("tao-moi")]
    public IActionResult Creating()
    {
        CourseDetailViewModel model = new CourseDetailViewModel
        {
            IsForCreating = true
        };
        return View("~/Views/Admin/Course/Course.cshtml", model);
    }

    [HttpPost("tao-moi")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Creating(CourseDetailViewModel model)
    {
        // Delete unnecessary sections and photos which has been
        // created and deleted before being saved into the database
        model.Sections?.RemoveAll(section => !section.Id.HasValue && section.IsDeleted);
        model.Photos?.RemoveAll(photo => !photo.Id.HasValue && photo.IsDeleted);

        // Map sections
        List<CourseSectionRequestDto> sectionRequestDtos = model.Sections?
            .Select(section => new CourseSectionRequestDto
            {
                Content = section.Content,
                IsDeleted = section.IsDeleted
            }).ToList();

        // Map photos
        List<CoursePhotoRequestDto> photoRequestDtos = new List<CoursePhotoRequestDto>();
        if (model.Photos != null)
        {
            foreach (CoursePhotoViewModel photo in model.Photos)
            {
                byte[] file = null;
                if (photo.File != null)
                {
                    using MemoryStream stream = new MemoryStream();
                    await photo.File.CopyToAsync(stream);
                    file = stream.ToArray();
                }
                photoRequestDtos.Add(new CoursePhotoRequestDto
                {
                    File = file,
                    IsDeleted = photo.IsDeleted
                });
            }
        }

        // Map course
        byte[] thumbnailFile = null;
        if (model.ThumbnailFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await model.ThumbnailFile.CopyToAsync(stream);
            thumbnailFile = stream.ToArray();
        }
        CourseRequestDto requestDto = new CourseRequestDto
        {
            Name = model.Name,
            Summary = model.Summary,
            Detail = model.Detail,
            ThumbnailUrl = model.ThumbnailUrl,
            ThumbnailFile = thumbnailFile,
            ThumbnailChanged = model.ThumbnailChanged,
            Sections = sectionRequestDtos,
            Photos = photoRequestDtos
        };

        // Call service for create operation
        ServiceResult<CourseDetailResponseDto> serviceResult;
        serviceResult = await _courseService.CreateAsync(requestDto);
        if (!serviceResult.Succeeded)
        {
            // Ensure the operation error doesn't occur, which is due to client-side
            if (serviceResult.HasOperationError)
            {
                return BadRequest();
            }

            // Notify clients that update operation failed with some validation errors
            ModelState.AddModelErrorsFromServiceErrors(serviceResult.Errors);
            return BadRequest(ModelState);
        }

        // Notify clients that operation has been done successfully
        return Ok();
    }

    [HttpGet("{id:int}/cap-nhat")]
    public async Task<IActionResult> Updating(int id)
    {
        ServiceResult<CourseDetailResponseDto> serviceResult;
        serviceResult = await _courseService.GetAsync(id);
        if (!serviceResult.Succeeded)
        {
            return NotFound();
        }

        CourseDetailViewModel model = new CourseDetailViewModel
        {
            Id = serviceResult.ResponseDto.Id,
            Name = serviceResult.ResponseDto.Name,
            Summary = serviceResult.ResponseDto.Summary,
            Detail = serviceResult.ResponseDto.Detail,
            ThumbnailUrl = serviceResult.ResponseDto.ThumbnailUrl,
            Sections = serviceResult.ResponseDto.Sections?
                .Select(s => new CourseSectionViewModel
                {
                    Id = s.Id,
                    Content = s.Content
                }).ToList(),
            Photos = serviceResult.ResponseDto.Photos?
                .Select(p => new CoursePhotoViewModel
                {
                    Id = p.Id,
                    Url = p.Url
                }).ToList()
        };
        return View("~/Views/Admin/Course/Course.cshtml", model);
    }

    [HttpPost("{id:int}/cap-nhat")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Updating(int id, CourseDetailViewModel model)
    {
        // Delete unnecessary sections and photos which has been
        // created and deleted before being saved into the database
        model.Sections?.RemoveAll(section => !section.Id.HasValue && section.IsDeleted);
        model.Photos?.RemoveAll(photo => !photo.Id.HasValue && photo.IsDeleted);

        // Map sections
        List<CourseSectionRequestDto> sectionRequestDtos = model.Sections?
            .Select(section => new CourseSectionRequestDto
            {
                Id = section.Id,
                Content = section.Content,
                IsDeleted = section.IsDeleted
            }).ToList();

        // Map photos
        List<CoursePhotoRequestDto> photoRequestDtos = new List<CoursePhotoRequestDto>();
        if (model.Photos != null)
        {
            foreach (CoursePhotoViewModel photo in model.Photos)
            {
                byte[] file = null;
                if (photo.File != null)
                {
                    using MemoryStream stream = new MemoryStream();
                    await photo.File.CopyToAsync(stream);
                    file = stream.ToArray();
                }
                photoRequestDtos.Add(new CoursePhotoRequestDto
                {
                    Id = photo.Id,
                    File = file,
                    IsDeleted = photo.IsDeleted
                });
            }
        }

        // Map course
        byte[] thumbnailFile = null;
        if (model.ThumbnailFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await model.ThumbnailFile.CopyToAsync(stream);
            thumbnailFile = stream.ToArray();
        }
        CourseRequestDto requestDto = new CourseRequestDto
        {
            Name = model.Name,
            Summary = model.Summary,
            Detail = model.Detail,
            ThumbnailUrl = model.ThumbnailUrl,
            ThumbnailFile = thumbnailFile,
            ThumbnailChanged = model.ThumbnailChanged,
            Sections = sectionRequestDtos,
            Photos = photoRequestDtos
        };

        // Call service for update operation
        ServiceResult<CourseDetailResponseDto> serviceResult;
        serviceResult = await _courseService.UpdateAsync(id, requestDto);
        if (!serviceResult.Succeeded)
        {
            // Ensure the course with given id exists
            if (serviceResult.HasNotFoundError)
            {
                return NotFound();
            }

            // Ensure the operation error doesn't occur, which is due to client-side
            if (serviceResult.HasOperationError)
            {
                return BadRequest();
            }

            // Notify clients that update operation failed with some validation errors
            ModelState.AddModelErrorsFromServiceErrors(serviceResult.Errors);
            return BadRequest(ModelState);
        }

        // Notify clients that update operation has been done successfully
        return Ok();
    }

    [HttpPost("{id:int}/xoa-bo")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deleting(int id)
    {
        ServiceResult<int> serviceResult = await _courseService.DeleteAsync(id);
        if (!serviceResult.Succeeded)
        {
            return NotFound();
        }

        return Ok();
    }
}