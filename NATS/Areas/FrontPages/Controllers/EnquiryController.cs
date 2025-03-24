using NATS.FrontPages.Models;

namespace NATS.FrontPages.Controllers;

[Area("FrontPages")]
[Route("cau-hoi")]
public class EnquiryController : Controller
{
    private readonly IEnquiryService _service;
    private readonly IValidator<EnquiryCreateRequestDto> _validator;

    public EnquiryController(
            IEnquiryService service,
            IValidator<EnquiryCreateRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    [HttpPost]
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
        return RedirectToAction("Success");
    }
    
    [HttpGet("thanh-cong")]
    public IActionResult Success()
    {
        return View();
    }
}