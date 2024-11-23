namespace NATS.Controllers;

[Route("quan-tri/noi-dung/dich-vu")]
[Authorize]
public class AdminBusinessServiceController : Controller
{
    private readonly IBusinessServiceService _businessServiceService;

    public AdminBusinessServiceController(IBusinessServiceService businessServiceService)
    {
        _businessServiceService = businessServiceService;   
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        // Fetch for service list
        ServiceResult<List<BusinessServiceBasicResponseDto>> serviceResult;
        serviceResult = await _businessServiceService.GetBasicListAsync();

        // Initialize view model
        BusinessServiceBasicListViewModel model = new BusinessServiceBasicListViewModel
        {
            Items = serviceResult.ResponseDto
                .Select(bs => new BusinessServiceBasicViewModel
                {
                    Id = bs.Id,
                    Name = bs.Name,
                    Summary = bs.Summary,
                    ThumbnailUrl = bs.ThumbnailUrl
                }).ToList()
        };

        return View("~/Views/Admin/BusinessService/BusinessServiceList.cshtml", model);
    }

    [HttpGet("tao-moi")]
    public IActionResult Creating()
    {
        BusinessServiceDetailViewModel model = new BusinessServiceDetailViewModel
        {
            IsForCreating = true
        };

        return View("~/Views/Admin/BusinessService/BusinessService.cshtml", model);
    }

    [HttpPost("tao-moi")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Creating(BusinessServiceDetailViewModel model)
    {
        // Delete unnecessary features and photos which has been
        // created and deleted before being saved into the database
        model.Features?.RemoveAll(feature => !feature.Id.HasValue && feature.IsDeleted);
        model.Photos?.RemoveAll(photo => !photo.Id.HasValue && photo.IsDeleted);

        // Map features
        List<BusinessServiceFeatureRequestDto> featureRequestDtos = model.Features?
            .Select(feature => new BusinessServiceFeatureRequestDto
            {
                Content = feature.Content,
                IsDeleted = feature.IsDeleted
            }).ToList();

        // Map photos
        List<BusinessServicePhotoRequestDto> photoRequestDtos = new List<BusinessServicePhotoRequestDto>();
        if (model.Photos != null)
        {
            foreach (BusinessServicePhotoViewModel photo in model.Photos)
            {
                byte[] file = null;
                if (photo.File != null)
                {
                    using MemoryStream stream = new MemoryStream();
                    await photo.File.CopyToAsync(stream);
                    file = stream.ToArray();
                }
                photoRequestDtos.Add(new BusinessServicePhotoRequestDto
                {
                    File = file,
                    IsDeleted = photo.IsDeleted
                });
            }
        }

        // Map business service
        byte[] thumbnailFile = null;
        if (model.ThumbnailFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await model.ThumbnailFile.CopyToAsync(stream);
            thumbnailFile = stream.ToArray();
        }
        BusinessServiceRequestDto requestDto = new BusinessServiceRequestDto
        {
            Name = model.Name,
            Summary = model.Summary,
            Detail = model.Detail,
            ThumbnailUrl = model.ThumbnailUrl,
            ThumbnailFile = thumbnailFile,
            ThumbnailChanged = model.ThumbnailChanged,
            Features = featureRequestDtos,
            Photos = photoRequestDtos
        };

        // Call service for create operation
        ServiceResult<BusinessServiceDetailResponseDto> serviceResult;
        serviceResult = await _businessServiceService.CreateAsync(requestDto);
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
        ServiceResult<BusinessServiceDetailResponseDto> serviceResult;
        serviceResult = await _businessServiceService.GetAsync(id);
        if (!serviceResult.Succeeded)
        {
            return NotFound();
        }
        
        // Map response dto properties to view mode
        BusinessServiceDetailViewModel model = new BusinessServiceDetailViewModel
        {
            Id = serviceResult.ResponseDto.Id,
            Name = serviceResult.ResponseDto.Name,
            Summary = serviceResult.ResponseDto.Summary,
            Detail = serviceResult.ResponseDto.Detail,
            ThumbnailUrl = serviceResult.ResponseDto.ThumbnailUrl,
            Features = serviceResult.ResponseDto.Features
                .Select(bsf => new BusinessServiceFeatureViewModel
                {
                    Id = bsf.Id,
                    Content = bsf.Content
                }).ToList(),
            Photos = serviceResult.ResponseDto.Photos
                .Select(bsp => new BusinessServicePhotoViewModel
                {
                    Id = bsp.Id,
                    Url = bsp.Url
                }).ToList()
        };
        
        return View("~/Views/Admin/BusinessService/BusinessService.cshtml", model);
    }

    [HttpPost("{id:int}/cap-nhat")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Updating(int id, BusinessServiceDetailViewModel model)
    {
        // Delete unnecessary features and photos which has been
        // created and deleted before being saved into the database
        model.Features?.RemoveAll(section => !section.Id.HasValue && section.IsDeleted);
        model.Photos?.RemoveAll(photo => !photo.Id.HasValue && photo.IsDeleted);

        // Map features
        List<BusinessServiceFeatureRequestDto> featureREquestDtos = model.Features?
            .Select(section => new BusinessServiceFeatureRequestDto
            {
                Id = section.Id,
                Content = section.Content,
                IsDeleted = section.IsDeleted
            }).ToList();

        // Map photos
        List<BusinessServicePhotoRequestDto> photoRequestDtos = new List<BusinessServicePhotoRequestDto>();
        if (model.Photos != null)
        {
            foreach (BusinessServicePhotoViewModel photo in model.Photos)
            {
                byte[] file = null;
                if (photo.File != null)
                {
                    using MemoryStream stream = new MemoryStream();
                    await photo.File.CopyToAsync(stream);
                    file = stream.ToArray();
                }
                photoRequestDtos.Add(new BusinessServicePhotoRequestDto
                {
                    Id = photo.Id,
                    File = file,
                    IsDeleted = photo.IsDeleted
                });
            }
        }

        // Map business service
        byte[] thumbnailFile = null;
        if (model.ThumbnailFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await model.ThumbnailFile.CopyToAsync(stream);
            thumbnailFile = stream.ToArray();
        }
        BusinessServiceRequestDto requestDto = new BusinessServiceRequestDto
        {
            Name = model.Name,
            Summary = model.Summary,
            Detail = model.Detail,
            ThumbnailUrl = model.ThumbnailUrl,
            ThumbnailFile = thumbnailFile,
            ThumbnailChanged = model.ThumbnailChanged,
            Features = featureREquestDtos,
            Photos = photoRequestDtos
        };

        // Call service for update operation
        ServiceResult<BusinessServiceDetailResponseDto> serviceResult;
        serviceResult = await _businessServiceService.UpdateAsync(id, requestDto);
        if (!serviceResult.Succeeded)
        {
            // Ensure the business service with given id exists
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
        ServiceResult<int> serviceResult = await _businessServiceService.DeleteAsync(id);
        if (!serviceResult.Succeeded)
        {
            return NotFound();
        }

        return Ok();
    }
}