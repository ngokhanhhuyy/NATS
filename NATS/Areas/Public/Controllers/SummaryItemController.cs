using NATS.Public.Models;

namespace NATS.Public.Controllers;

[Area("Public")]
[Route("/gioi-thieu")]
public class SummaryItemController : Controller
{
    private readonly ISummaryItemService _service;

    public const string SummaryItemListRouteName = "PublicSummaryItemList";

    public SummaryItemController(ISummaryItemService service)
    {
        _service = service;
    }

    [HttpGet("{id:int?}", Name = SummaryItemListRouteName)]
    public async Task<IActionResult> Index(int? id = null)
    {
        List<SummaryItemResponseDto> responseDtos = await _service.GetListAsync();
        SummaryItemListViewModel model;
        model = new SummaryItemListViewModel(responseDtos, id);

        return View(model);
    }
}