namespace NATS.Components;

public class CurrentUserAccount : ViewComponent
{
    private readonly IAuthorizationService _authorizationService;

    public CurrentUserAccount(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        UserDetailResponseDto responseDto = await _authorizationService.GetCallerUserDetailAsync();
        UserDetailModel model = new UserDetailModel(responseDto);
        return View(model);
    }
}