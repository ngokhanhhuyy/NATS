namespace NATS.Controllers;

[Authorize(AuthenticationSchemes = "Identity.Application")]
public class AuthenticationController : Controller
{
    private readonly IAuthenticationService _service;
    private readonly IValidator<SignInRequestDto> _validator;

    public AuthenticationController(IAuthenticationService service)
    {
        _service = service;
    }

    [HttpGet("Login")]
    [AllowAnonymous]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Dashboard", "Admin");
        }

        SignInModel model = new SignInModel();
        return View(model);
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(SignInModel model, [FromQuery] string returningUrl)
    {
        // Check if the user is already authenticated.
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Dashboard", "Admin");
        }

        // Validate the data from the request.
        SignInRequestDto requestDto = model.ToRequestDto();
        ValidationResult validationResult = _validator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return View(new SignInModel());
        }

        SignInModel validatedModel = new SignInModel
        {
            WasValidated = true
        };

        try
        {
            await _service.SignInAsync(requestDto);
            if (returningUrl != null)
            {
                return Redirect(returningUrl);
            }

            return RedirectToAction("Dashboard", "Admin");
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return View(validatedModel);
        }
    }

    [HttpGet("Logout")]
    public async Task<IActionResult> Logout()
    {
        await _service.SignOutAsync();
        return RedirectToAction("SignIn", "Authentication");
    }
}