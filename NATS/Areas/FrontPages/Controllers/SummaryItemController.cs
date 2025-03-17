using NATS.FrontPages.Models;

namespace NATS.FrontPages.Controllers;

[Area("FrontPages")]
[Route("/gioi-thieu")]
public class SummaryItemController : Controller
{
    private readonly ISummaryItemService _service;

    public SummaryItemController(ISummaryItemService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<SummaryItemResponseDto> responseDtos = await _service.GetListAsync();
        SummaryItemListViewModel model = new SummaryItemListViewModel(responseDtos);

        return View(model);
    }
}