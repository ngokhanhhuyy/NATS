using NATS.Protected.Models;

namespace NATS.Protected.Controllers;

[Area("Protected")]
[Route("quan-tri/trinh-chieu-anh")]
[Authorize]
public class SliderItemController : Controller
{
    private readonly ISliderItemService _service;
    private readonly IValidator<SliderItemUpsertRequestDto> _validator;

    public const string ListRouteName = "ProtectedSliderItemList";
    public const string CreateRouteName = "ProtectedSliderItemCreate";
    public const string UpdateRouteName = "ProtectedSliderItemUpdate";

    public SliderItemController(
            ISliderItemService service,
            IValidator<SliderItemUpsertRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    [HttpGet(Name = ListRouteName)]
    public async Task<IActionResult> List()
    {
        List<SliderItemResponseDto> responseDtos = await _service.GetListAsync();
        List<SliderItemDetailModel> model = responseDtos
            .Select(dto => new SliderItemDetailModel(dto))
            .ToList();

        return View(model);
    }

    [HttpGet("tao-moi", Name = CreateRouteName)]
    public IActionResult Create()
    {
        SliderItemUpsertViewModel model = new SliderItemUpsertViewModel();
        return View("Upsert", model);
    }

    [HttpGet("{id:int}", Name = UpdateRouteName)]
    public async Task<IActionResult> Update(int id)
    {
        try
        {
            SliderItemResponseDto responseDto = await _service.GetSingleAsync(id);
            SliderItemUpsertViewModel model = new SliderItemUpsertViewModel();
            model.MapFromResponseDto(responseDto);

            return View("Upsert", model);
        }
        catch (ResourceNotFoundException)
        {
            return Redirect("/Error");
        }
    }
}