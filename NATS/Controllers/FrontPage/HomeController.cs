namespace NATS.Controllers;

[Route("/")]
public class HomeController : Controller
{
    private readonly ISliderItemService _sliderItemService;
    private readonly ISummaryItemService _summaryItemService;

    public HomeController(
            ISliderItemService sliderItemService,
            ISummaryItemService summaryItemService)
    {
        _sliderItemService = sliderItemService;
        _summaryItemService = summaryItemService;
    }

    [HttpGet]
    public async Task<IActionResult> HomePage()
    {
        Task<List<SliderItemResponseDto>> sliderItemResponseDtosTask;
        sliderItemResponseDtosTask = _sliderItemService.GetListAsync();
        
        Task<List<SummaryItemResponseDto>> summaryItemResponseDtosTask;
        summaryItemResponseDtosTask = _summaryItemService.GetListAsync();

        await Task.WhenAll(sliderItemResponseDtosTask, summaryItemResponseDtosTask);

        FrontPageHomeModel model = new FrontPageHomeModel
        {
            SliderItems = sliderItemResponseDtosTask.Result
                .Select(dto => new SliderItemDetailModel(dto))
                .ToList(),
            SummaryItems = summaryItemResponseDtosTask.Result
                .Select(dto => new SummaryItemDetailModel(dto))
                .ToList()
        };

        return View("~/Views/FrontPageHomeView.cshtml", model);
    }
}