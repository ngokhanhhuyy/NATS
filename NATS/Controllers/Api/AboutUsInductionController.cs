namespace NATS.Controllers.Api;

[Route("/Api/[controller]")]
public class AboutUsIntroductionController : ControllerBase
{
    private readonly IAboutUsIntroductionService _service;
    private readonly IValidator<AboutUsIntroductionUpdateRequestDto> _validator;

    public AboutUsIntroductionController(
            IAboutUsIntroductionService service,
            IValidator<AboutUsIntroductionUpdateRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    [HttpGet]
    [ProducesResponseType<AboutUsIntroductionResponseDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Detail()
    {
        return Ok(await _service.GetAsync());
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(AboutUsIntroductionUpdateRequestDto requestDto)
    {
        requestDto.TransformValues();
        ValidationResult validationResult = _validator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ValidationProblem(ModelState));
        }

        try
        {
            await _service.UpdateAsync(requestDto);
            return Ok();
        }
        catch (ConcurrencyException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return Conflict(ValidationProblem(ModelState));
        }
    }
}