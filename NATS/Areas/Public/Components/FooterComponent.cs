using NATS.Public.Models;

namespace NATS.Public.Components;

public class FooterComponent : ViewComponent
{
    private readonly IGeneralSettingsService _generalSettingsService;
    private readonly IContactService _contactService;

    public FooterComponent(
            IGeneralSettingsService generalSettingsService,
            IContactService contactService)
    {
        _generalSettingsService = generalSettingsService;
        _contactService = contactService;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        Task<GeneralSettingsResponseDto> generalSettingsResponseDtoTask;
        generalSettingsResponseDtoTask = _generalSettingsService.GetAsync();

        Task<List<ContactResponseDto>> contactResponseDtosTask;
        contactResponseDtosTask = _contactService.GetListAsync();

        await Task.WhenAll(generalSettingsResponseDtoTask, contactResponseDtosTask);

        FooterViewModel model = new FooterViewModel(
            generalSettingsResponseDtoTask.Result,
            contactResponseDtosTask.Result);

        return View(model);
    }
}