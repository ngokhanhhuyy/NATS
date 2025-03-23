using NATS.FrontPages.Models;

namespace NATS.Areas.FrontPages;

public class NavigationBarComponent : ViewComponent
{
    private readonly IGeneralSettingsService _service;

    public NavigationBarComponent(IGeneralSettingsService service)
    {
        _service = service;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        GeneralSettingsResponseDto responseDto = await _service.GetAsync();
        NavigationBarViewModel model = new NavigationBarViewModel(responseDto);
        return View(model);
    }
}