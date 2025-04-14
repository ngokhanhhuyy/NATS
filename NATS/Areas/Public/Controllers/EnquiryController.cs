using NATS.Public.Models;

namespace NATS.Public.Controllers;

[Area("Public")]
[Route("cau-hoi")]
public class EnquiryController : Controller
{
    private readonly IEnquiryService _service;
    private readonly IValidator<EnquiryCreateRequestDto> _validator;

    public const string EnquiryRouteName = "PublicEnquiry";
    public const string CreateSuccessRouteName = "PublicEnquiryCreateSuccess";

    public EnquiryController(
            IEnquiryService service,
            IValidator<EnquiryCreateRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    [HttpPost(Name = EnquiryRouteName)]
    public async Task<IActionResult> Create(EnquiryViewModel model)
    {
        EnquiryCreateRequestDto requestDto = model.ToRequestDto();
        ValidationResult validationResult = _validator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            model.IsValidated = true;
            return View(model);
        }

        await _service.CreateAsync(requestDto);
        return RedirectToRoute(CreateSuccessRouteName);
    }
    
    [HttpGet("thanh-cong", Name = CreateSuccessRouteName)]
    public IActionResult Success()
    {
        return View();
    }
}