namespace NATS.Controllers;

[Authorize(AuthenticationSchemes = "Identity.Application")]
public class AuthenticationController : Controller
{
    private readonly IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("Login")]
    [AllowAnonymous]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Dashboard", "Admin");
        }

        LoginViewModel model = new LoginViewModel();
        return View(model);
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, [FromQuery] string returningUrl)
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Dashboard", "Admin");
        }

        ServiceResult<LoginResponseDto> serviceResult;
        serviceResult = await _userService.LoginAsync(new LoginRequestDto
        {
            UserName = model.UserName,
            Password = model.Password
        });

        if (!serviceResult.Succeeded)
        {
            ModelState.AddModelErrorsFromServiceErrors(serviceResult.Errors);
            model.WasValidated = true;
            return View(model);
        }

        if (returningUrl != null)
        {
            return Redirect(returningUrl);
        }
        return RedirectToAction("Dashboard", "Admin");
    }

    [HttpGet("Logout")]
    public async Task<IActionResult> Logout()
    {
        await _userService.LogoutAsync();
        return RedirectToAction("Login", "Authentication");
    }
}