using NATS.FrontPages.Models;

namespace NATS.Areas.FrontPages.Controllers;

[Area("FrontPages")]
[Route("/")]
public class HomeController : Controller
{
    private readonly ISliderItemService _sliderItemService;
    private readonly ISummaryItemService _summaryItemService;
    private readonly IGeneralSettingsService _generalSettingsService;
    private readonly IAboutUsIntroductionService _aboutUsIntroductionService;
    private readonly ICatalogItemService _catalogItemService;
    private readonly IContactService _contactService;

    public HomeController(
            ISliderItemService sliderItemService,
            ISummaryItemService summaryItemService,
            IGeneralSettingsService generalSettingsService,
            IAboutUsIntroductionService aboutUsIntroductionService,
            ICatalogItemService catalogItemService,
            IContactService contactService)
    {
        _sliderItemService = sliderItemService;
        _summaryItemService = summaryItemService;
        _generalSettingsService = generalSettingsService;
        _aboutUsIntroductionService = aboutUsIntroductionService;
        _catalogItemService = catalogItemService;
        _contactService = contactService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        Task<GeneralSettingsResponseDto> generalSettingsResponseDtoTask;
        generalSettingsResponseDtoTask = _generalSettingsService.GetAsync();

        Task<List<SliderItemResponseDto>> sliderItemResponseDtosTask;
        sliderItemResponseDtosTask = _sliderItemService.GetListAsync();
        
        Task<List<SummaryItemResponseDto>> summaryItemResponseDtosTask;
        summaryItemResponseDtosTask = _summaryItemService.GetListAsync();

        Task<AboutUsIntroductionResponseDto> aboutUsIntroductionResponseDtoTask;
        aboutUsIntroductionResponseDtoTask = _aboutUsIntroductionService.GetAsync();

        Task<List<CatalogItemBasicResponseDto>> serviceResponseDtosTask;
        serviceResponseDtosTask = _catalogItemService.GetListAsync();

        Task<List<ContactResponseDto>> contactResponseDtosTask;
        contactResponseDtosTask = _contactService.GetListAsync();

        await Task.WhenAll(
            generalSettingsResponseDtoTask,
            sliderItemResponseDtosTask,
            summaryItemResponseDtosTask);

        HomeViewModel model = new HomeViewModel
        {
            SliderItems = sliderItemResponseDtosTask.Result
                .Select(dto => new SliderItemDetailModel(dto))
                .ToList(),
            SummaryItems = summaryItemResponseDtosTask.Result
                .Select(dto => new SummaryItemDetailModel(dto))
                .ToList(),
            AboutUsIntroduction = new AboutUsIntroductionDetailModel(
                aboutUsIntroductionResponseDtoTask.Result),
            Services = serviceResponseDtosTask.Result
                .Where(dto => dto.Type == CatalogItemType.Service)
                .Select(dto => new CatalogItemBasicModel(dto))
                .ToList(),
            Courses = serviceResponseDtosTask.Result
                .Where(dto => dto.Type == CatalogItemType.Course)
                .Select(dto => new CatalogItemBasicModel(dto))
                .ToList(),
            Products = serviceResponseDtosTask.Result
                .Where(dto => dto.Type == CatalogItemType.Product)
                .Select(dto => new CatalogItemBasicModel(dto))
                .ToList(),
            Contacts = contactResponseDtosTask.Result
                .Select(dto => new ContactDetailModel(dto))
                .ToList(),
            GeneralSettings = new GeneralSettingsDetailModel(
                generalSettingsResponseDtoTask.Result)
        };

        return View(model);
    }
}