using NATS.FrontPages.Models;

namespace NATS.FrontPages.Controllers;

[Area("FrontPages")]
[Route("/lien-he")]
public class ContactController : Controller
{
    private readonly IContactService _contactService;
    private readonly IGeneralSettingsService _generalSettingsService;

    public ContactController(
            IContactService contactService,
            IGeneralSettingsService generalSettingsService)
    {
        _contactService = contactService;
        _generalSettingsService = generalSettingsService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        Task<List<ContactResponseDto>> contactResponseDtosTask;
        contactResponseDtosTask = _contactService.GetListAsync();

        Task<GeneralSettingsResponseDto> generalSettingsResponseDtoTask;
        generalSettingsResponseDtoTask = _generalSettingsService.GetAsync();

        await Task.WhenAll(contactResponseDtosTask, generalSettingsResponseDtoTask);

        ContactViewModel model = new ContactViewModel(
            contactResponseDtosTask.Result,
            generalSettingsResponseDtoTask.Result);

        return View(model);
    }
}