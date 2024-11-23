namespace NATS.Controllers;

[Route("quan-tri/noi-dung/trinh-chieu-anh")]
public class AdminHomePageSliderItemController : Controller
{
    private readonly IHomePageSliderItemService _service;

    public AdminHomePageSliderItemController(IHomePageSliderItemService service)
    {
        _service = service;
    }

    [HttpGet("tao-moi")]
    public IActionResult Creating()
    {
        HomePageSliderItemViewModel model = new HomePageSliderItemViewModel
        {
            IsForCreating = true
        };
        return View("~/Views/Admin/HomePageSliderItem.cshtml", model);
    }

    [HttpPost("tao-moi")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Creating(HomePageSliderItemViewModel model)
    {
        byte[] photoFile = null;
        if (model.PhotoFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await model.PhotoFile.CopyToAsync(stream);
            photoFile = stream.ToArray();
        }
        
        // Map request data to request dto
        HomePageSliderItemRequestDto requestDto = new HomePageSliderItemRequestDto
        {
            Title = model.Title,
            PhotoFile = photoFile,
            PhotoChanged = model.PhotoChanged
        };

        // Perform creating operation
        ServiceResult<HomePageSliderItemResponseDto> serviceResult;
        serviceResult = await _service.CreateAsync(requestDto);
        if (!serviceResult.Succeeded)
        {
            ModelState.AddModelErrorsFromServiceErrors(serviceResult.Errors);
            return BadRequest(ModelState);
        }
        return Ok();
    }

    [HttpGet("{id:int}/cap-nhat")]
    public async Task<IActionResult> Updating(int id)
    {
        ServiceResult<HomePageSliderItemResponseDto> serviceResult;
        serviceResult = await _service.GetAsync(id);
        if (!serviceResult.Succeeded)
        {
            return NotFound();
        }

        // Map response dto to model
        HomePageSliderItemViewModel model = new HomePageSliderItemViewModel
        {
            Id = serviceResult.ResponseDto.Id,
            Title = serviceResult.ResponseDto.Title,
            PhotoUrl = serviceResult.ResponseDto.PhotoUrl
        };
        return View("~/Views/Admin/HomePageSliderItem.cshtml", model);
    }

    [HttpPost("{id:int}/cap-nhat")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Updating(int id, HomePageSliderItemViewModel model)
    {
        byte[] photoFile = null;
        if (model.PhotoFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await model.PhotoFile.CopyToAsync(stream);
            photoFile = stream.ToArray();
        }
        
        // Map request data to request dto
        HomePageSliderItemRequestDto requestDto = new HomePageSliderItemRequestDto
        {
            Title = model.Title,
            PhotoFile = photoFile,
            PhotoChanged = model.PhotoChanged,
        };

        // Perform updating operation
        ServiceResult<HomePageSliderItemResponseDto> serviceResult;
        serviceResult = await _service.UpdateAsync(id, requestDto);
        if (!serviceResult.Succeeded)
        {
            if (serviceResult.HasNotFoundError)
            {
                return NotFound();
            }
            if (serviceResult.HasOperationError)
            {
                return BadRequest();
            }
            ModelState.AddModelErrorsFromServiceErrors(serviceResult.Errors);
            return BadRequest(ModelState);
        }
        return Ok();
    }

    [HttpPost("{id:int}/xoa-bo")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deleting(int id)
    {
        ServiceResult<int> serviceResult = await _service.DeleteAsync(id);
        if (!serviceResult.Succeeded)
        {
            return NotFound();
        }
        return Ok();
    }
}