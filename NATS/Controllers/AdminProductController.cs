namespace NATS.Controllers;

[Route("quan-tri/noi-dung/san-pham")]
[Authorize]
public class AdminProductController : Controller
{
    private readonly IProductService _productService;

    public AdminProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        ServiceResult<List<ProductBasicResponseDto>> serviceResult;
        serviceResult = await _productService.GetBasicListAsync();

        ProductBasicListViewModel model = new ProductBasicListViewModel
        {
            Items = serviceResult.ResponseDto
                .Select(c => new ProductBasicViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Summary = c.Summary,
                    ThumbnailUrl = c.ThumbnailUrl
                }).ToList()
        };
        return View("~/Views/Admin/Product/ProductList.cshtml", model);
    }

    [HttpGet("tao-moi")]
    public IActionResult Creating()
    {
        ProductDetailViewModel model = new ProductDetailViewModel
        {
            IsForCreating = true
        };
        return View("~/Views/Admin/Product/Product.cshtml", model);
    }

    [HttpPost("tao-moi")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Creating(ProductDetailViewModel model)
    {
        // Delete unnecessary features and photos which has been
        // created and deleted before being saved into the database
        model.Features?.RemoveAll(feature => !feature.Id.HasValue && feature.IsDeleted);
        model.Photos?.RemoveAll(photo => !photo.Id.HasValue && photo.IsDeleted);

        // Map features
        List<ProductFeatureRequestDto> featureRequestDtos = model.Features?
            .Select(feature => new ProductFeatureRequestDto
            {
                Content = feature.Content,
                IsDeleted = feature.IsDeleted
            }).ToList();

        // Map photos
        List<ProductPhotoRequestDto> photoRequestDtos = new List<ProductPhotoRequestDto>();
        if (model.Photos != null)
        {
            foreach (ProductPhotoViewModel photo in model.Photos)
            {
                byte[] file = null;
                if (photo.File != null)
                {
                    using MemoryStream stream = new MemoryStream();
                    await photo.File.CopyToAsync(stream);
                    file = stream.ToArray();
                }
                photoRequestDtos.Add(new ProductPhotoRequestDto
                {
                    File = file,
                    IsDeleted = photo.IsDeleted
                });
            }
        }

        // Map product
        byte[] thumbnailFile = null;
        if (model.ThumbnailFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await model.ThumbnailFile.CopyToAsync(stream);
            thumbnailFile = stream.ToArray();
        }
        ProductRequestDto requestDto = new ProductRequestDto
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
        ServiceResult<ProductDetailResponseDto> serviceResult;
        serviceResult = await _productService.CreateAsync(requestDto);
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
        ServiceResult<ProductDetailResponseDto> serviceResult;
        serviceResult = await _productService.GetAsync(id);
        if (!serviceResult.Succeeded)
        {
            return NotFound();
        }

        ProductDetailViewModel model = new ProductDetailViewModel
        {
            Id = serviceResult.ResponseDto.Id,
            Name = serviceResult.ResponseDto.Name,
            Summary = serviceResult.ResponseDto.Summary,
            Detail = serviceResult.ResponseDto.Detail,
            ThumbnailUrl = serviceResult.ResponseDto.ThumbnailUrl,
            Features = serviceResult.ResponseDto.Features?
                .Select(s => new ProductFeatureViewModel
                {
                    Id = s.Id,
                    Content = s.Content
                }).ToList(),
            Photos = serviceResult.ResponseDto.Photos?
                .Select(p => new ProductPhotoViewModel
                {
                    Id = p.Id,
                    Url = p.Url
                }).ToList()
        };
        return View("~/Views/Admin/Product/Product.cshtml", model);
    }

    [HttpPost("{id:int}/cap-nhat")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Updating(int id, ProductDetailViewModel model)
    {
        // Delete unnecessary features and photos which has been
        // created and deleted before being saved into the database
        model.Features?.RemoveAll(feature => !feature.Id.HasValue && feature.IsDeleted);
        model.Photos?.RemoveAll(photo => !photo.Id.HasValue && photo.IsDeleted);

        // Map features
        List<ProductFeatureRequestDto> featureRequestDtos = model.Features?
            .Select(feature => new ProductFeatureRequestDto
            {
                Id = feature.Id,
                Content = feature.Content,
                IsDeleted = feature.IsDeleted
            }).ToList();

        // Map photos
        List<ProductPhotoRequestDto> photoRequestDtos = new List<ProductPhotoRequestDto>();
        if (model.Photos != null)
        {
            foreach (ProductPhotoViewModel photo in model.Photos)
            {
                byte[] file = null;
                if (photo.File != null)
                {
                    using MemoryStream stream = new MemoryStream();
                    await photo.File.CopyToAsync(stream);
                    file = stream.ToArray();
                }
                photoRequestDtos.Add(new ProductPhotoRequestDto
                {
                    Id = photo.Id,
                    File = file,
                    IsDeleted = photo.IsDeleted
                });
            }
        }

        // Map product
        byte[] thumbnailFile = null;
        if (model.ThumbnailFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await model.ThumbnailFile.CopyToAsync(stream);
            thumbnailFile = stream.ToArray();
        }
        ProductRequestDto requestDto = new ProductRequestDto
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

        // Call service for update operation
        ServiceResult<ProductDetailResponseDto> serviceResult;
        serviceResult = await _productService.UpdateAsync(id, requestDto);
        if (!serviceResult.Succeeded)
        {
            // Ensure the product with given id exists
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
        ServiceResult<int> serviceResult = await _productService.DeleteAsync(id);
        if (!serviceResult.Succeeded)
        {
            return NotFound();
        }

        return Ok();
    }
}