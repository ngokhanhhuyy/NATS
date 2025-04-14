using NATS.Public.Models;

namespace NATS.Public.Controllers;

[Area("Public")]
[Route("/lien-he")]
public class ContactController : Controller
{
    private readonly IContactService _contactService;
    private readonly IGeneralSettingsService _generalSettingsService;

    public const string ContactRouteName = "PublicContact";

    public ContactController(
            IContactService contactService,
            IGeneralSettingsService generalSettingsService)
    {
        _contactService = contactService;
        _generalSettingsService = generalSettingsService;
    }

    [HttpGet(Name = ContactRouteName)]
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