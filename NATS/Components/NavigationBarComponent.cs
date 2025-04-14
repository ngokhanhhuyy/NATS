using NATS.Public.Models;

namespace NATS.Areas.Public;

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