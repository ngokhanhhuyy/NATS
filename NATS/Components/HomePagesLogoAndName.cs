namespace NATS.Components;

public class HomePagesLogoAndName : ViewComponent
{
    private readonly IGeneralSettingsService _generalSettingsService;

    public HomePagesLogoAndName(IGeneralSettingsService generalSettingsService)
    {
        _generalSettingsService = generalSettingsService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        GeneralSettingsResponseDto responseDto = await _generalSettingsService.GetAsync();
        GeneralSettingsDetailModel model = new GeneralSettingsDetailModel(responseDto);
        
        return View(model);
    }
}