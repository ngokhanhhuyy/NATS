namespace NATS.Controllers;

[Route("quan-tri/noi-dung/catalog")]
[Authorize]
public class AdminCatalogController : Controller
{
    private readonly ICatalogItemService _service;
    private readonly IValidator<CatalogItemUpsertRequestDto> _upsertValidator;

    public AdminCatalogController(
            ICatalogItemService service,
            IValidator<CatalogItemUpsertRequestDto> upsertValidator)
    {
        _service = service;
        _upsertValidator = upsertValidator;
    }

    [HttpGet]
    public async Task<IActionResult> List(CatalogItemType type)
    {
        List<CatalogItemBasicResponseDto> responseDtos = await _service.GetListAsync(type);
        List<CatalogItemBasicModel> model = responseDtos
            .Select(dto => new CatalogItemBasicModel(dto))
            .ToList();

        return View("~/Views/Admin/CatalogItem/CatalogItemList.cshtml", model);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Detail(int id)
    {
        try
        {
            CatalogItemDetailResponseDto responseDto = await _service.GetDetailAsync(id);
            CatalogItemDetailModel model = new CatalogItemDetailModel(responseDto);
            return View("~/Views/Admin/CatalogItem/CatalogItemDetail.cshtml", model);
        }
        catch (ResourceNotFoundException)
        {
            return RedirectToAction("List", "CatalogItem");
        }
    }

    [HttpGet("tao-moi")]
    public IActionResult Creating()
    {
        CatalogItemUpsertModel model = new CatalogItemUpsertModel
        {
            IsForCreating = true
        };

        return View("~/Views/Admin/CatalogItem/CatalogItemUpsert.cshtml", model);
    }

    [HttpPost("tao-moi")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Creating(CatalogItemUpsertModel model)
    {
        // Delete unnecessary photos which have been created and deleted before being saved
        // into the database.
        model.RemoveUnnecessaryPhotos();

        // Map photos.
        List<CatalogItemUpsertPhotoRequestDto> photoRequestDtos;
        photoRequestDtos = new List<CatalogItemUpsertPhotoRequestDto>();
        if (model.Photos != null)
        {
            foreach (CatalogItemUpsertPhotoModel photo in model.Photos)
            {
                byte[] file = null;
                if (photo.File != null)
                {
                    using MemoryStream stream = new MemoryStream();
                    await photo.File.CopyToAsync(stream);
                    file = stream.ToArray();
                }

                photoRequestDtos.Add(new CatalogItemUpsertPhotoRequestDto
                {
                    File = file,
                    IsDeleted = photo.IsDeleted
                });
            }
        }

        // Map catalog item.
        byte[] thumbnailFile = null;
        if (model.ThumbnailFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await model.ThumbnailFile.CopyToAsync(stream);
            thumbnailFile = stream.ToArray();
        }
        
        // Initialize request DTO.
        CatalogItemUpsertRequestDto requestDto;
        requestDto = (await model.ToRequestDto()).TransformValues();

        // Validate data bound from the request.
        ValidationResult validationResult;
        validationResult = _upsertValidator.Validate(requestDto, options =>
        {
            options.IncludeRuleSets("Create").IncludeRulesNotInRuleSet();
        });
        
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return View("~/Views/Admin/CatalogItem/CatalogItemUpsert.cshtml", model);
        }

        // Call service for create operation.
        try
        {
            int createdId = await _service.CreateAsync(requestDto);
            return RedirectToAction("Detail", "AdminCatalogItem", new { id = createdId });
        }
        catch (ConcurrencyException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            model.Photos = new List<CatalogItemUpsertPhotoModel>();
            return View("~/Views/Admin/CatalogItem/CatalogItemUpsert.cshtml", model);
        }
    }

    [HttpGet("{id:int}/cap-nhat")]
    public async Task<IActionResult> Updating(int id)
    {
        CatalogItemDetailResponseDto responseDto = await _service.GetDetailAsync(id);
        CatalogItemDetailModel model = new CatalogItemDetailModel(responseDto);
        
        return View("~/Views/Admin/CatalogItem/CatalogItemUpsert.cshtml", model);
    }

    [HttpPost("{id:int}/cap-nhat")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Updating(int id, CatalogItemUpsertModel model)
    {
        // Delete unnecessary photos which have been created and deleted before being saved
        // into the database.
        model.RemoveUnnecessaryPhotos();

        // Initialize the request DTO.
        CatalogItemUpsertRequestDto requestDto;
        requestDto = (await model.ToRequestDto()).TransformValues();

        // Validate data bound from the request.
        ValidationResult validationResult;
        validationResult = _upsertValidator.Validate(requestDto, options =>
        {
            options.IncludeRuleSets("Update").IncludeRulesNotInRuleSet();
        });

        // Perform the operation.
        try
        {
            await _service.UpdateAsync(id, requestDto);
            return RedirectToAction("Detail", "AdminCatalogItem");
        }
        catch (ConcurrencyException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return View("~/Views/Admin/CatalogItem/CatalogItemUpsert.cshtml", model);
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return View("~/Views/Admin/CatalogItem/CatalogItemUpsert.cshtml", model);
        }
    }

    [HttpPost("{id:int}/xoa-bo")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deleting(int id)
    {
        await _service.DeleteAsync(id);
        return RedirectToAction("List", "AdminCatalogItem");
    }
}