using NATS.Protected.Models;

namespace NATS.Protected.Controllers;

[Area("Protected")]
[Route("quan-tri")]
public class AuthenticationController : Controller
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IGeneralSettingsService _generalSettingsService;
    private readonly IValidator<SignInRequestDto> _signInValidator;

    public const string SignInRouteName = "ProtectedSignIn";
    public const string SignOutRouteName = "ProtectedSignOut";

    public AuthenticationController(
            IAuthenticationService authenticationService,
            IGeneralSettingsService generalSettingsService,
            IValidator<SignInRequestDto> signInValidator)
    {
        _authenticationService = authenticationService;
        _generalSettingsService = generalSettingsService;
        _signInValidator = signInValidator;
    }

    [HttpGet("dang-nhap", Name = SignInRouteName)]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn()
    {
        GeneralSettingsResponseDto generalSettingsResponseDto;
        generalSettingsResponseDto = await _generalSettingsService.GetAsync();

        SignInViewModel model = new SignInViewModel
        {
            GeneralSettings = new GeneralSettingsDetailModel(generalSettingsResponseDto)
        };

        return View(model);
    }

    [HttpPost("dang-nhap", Name = SignInRouteName)]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn(SignInViewModel model)
    {
        GeneralSettingsResponseDto generalSettingsResponseDto;
        generalSettingsResponseDto = await _generalSettingsService.GetAsync();
        model.GeneralSettings = new GeneralSettingsDetailModel(generalSettingsResponseDto);
        SignInRequestDto requestDto = model.ToRequestDto();

        // Validate data from the request.
        ValidationResult validationResult = _signInValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            model.Password = string.Empty;
            return View(model);
        }

        try
        {
            await _authenticationService.SignInAsync(requestDto);
            return RedirectToRoute(DashboardController.DashboardRouteName);
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            model.Password = string.Empty;
            return View(model);
        }
    }

    [HttpPost("dang-xuat", Name = SignOutRouteName)]
    [Authorize]
    public new async Task<IActionResult> SignOut()
    {
        await _authenticationService.SignOutAsync();
        return Ok();
    }
}