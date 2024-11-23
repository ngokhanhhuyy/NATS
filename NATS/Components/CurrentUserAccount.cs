namespace NATS.Components;

public class CurrentUserAccount : ViewComponent
{
    private readonly IUserService _userService;

    public CurrentUserAccount(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        ClaimsPrincipal userPricipal = (ClaimsPrincipal)User;
        string nameIdentifier = userPricipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (nameIdentifier == null) {
            throw new NullReferenceException(userPricipal.FindFirstValue(ClaimTypes.Name));
        }
        int userId = int.Parse(nameIdentifier);
        await _userService.SetCurrentUserId(userId);
        ServiceResult<UserBasicResponseDto> serviceResult;
        serviceResult = _userService.GetUserAsCurrentUser();
        UserBasicViewModel model = new UserBasicViewModel
        {
            Id = serviceResult.ResponseDto.Id,
            UserName = serviceResult.ResponseDto.UserName,
            Role = new RoleViewModel
            {
                Id = serviceResult.ResponseDto.Role.Id,
                Name = serviceResult.ResponseDto.Role.Name,
                DisplayName = serviceResult.ResponseDto.Role.DisplayName,
            }
        };
        return View(model);
    }
}