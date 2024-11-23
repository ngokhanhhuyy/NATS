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
        ServiceResult<GeneralSettingsResponseDto> serviceResult;
        serviceResult = await _generalSettingsService.GetAsync();
        GeneralSettingsViewModel model = new GeneralSettingsViewModel
        {
            ApplicationName = serviceResult.ResponseDto.ApplicationName,
            ApplicationShortName = serviceResult.ResponseDto.ApplicationShortName
        };
        
        return View(model);
    }
}