namespace NATS.Controllers;

[Route("/quan-tri")]
public class AdminAuthenticationController : Controller
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IValidator<SignInRequestDto> _signInValidator;
    private readonly string _viewPath = "~/Admin/SignIn/SignInView.cshtml";

    public AdminAuthenticationController(
            IAuthenticationService authenticationService,
            IValidator<SignInRequestDto> signInValidator)
    {
        _authenticationService = authenticationService;
        _signInValidator = signInValidator;
    }

    [HttpGet("/dang-nhap")]
    [AllowAnonymous]
    public IActionResult SignIn()
    {
        SignInModel model = new SignInModel();
        return View(_viewPath, model);
    }

    [HttpPost("/dang-nhap")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn(SignInModel model)
    {
        SignInRequestDto requestDto = model.ToRequestDto();

        // Validate data from the request.
        ValidationResult validationResult = _signInValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return View(_viewPath, model);
        }

        try
        {
            await _authenticationService.SignInAsync(requestDto);
            return RedirectToAction("Dashboard", "AdminDashboard");
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return View(_viewPath, model);
        }
    }
}