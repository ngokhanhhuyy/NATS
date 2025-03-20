namespace NATS.Controllers.Api;

[Route("/Api/[controller]")]
public class GeneralSettingsController : Controller
{
    private readonly IGeneralSettingsService _service;
    private readonly IValidator<GeneralSettingsUpdateRequestDto> _validator;

    public GeneralSettingsController(
            IGeneralSettingsService service,
            IValidator<GeneralSettingsUpdateRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Detail()
    {
        return Ok(await _service.GetAsync());
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(GeneralSettingsUpdateRequestDto requestDto)
    {
        requestDto.TransformValues();
        ValidationResult validationResult = _validator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        try
        {
            await _service.UpdateAsync(requestDto);
            return Ok();
        }
        catch (ConcurrencyException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return Conflict(ModelState);
        }
    }
}