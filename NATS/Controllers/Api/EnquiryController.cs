namespace NATS.Controllers.Api;

[Route("Api/[controller]")]
public class EnquiryController : ControllerBase
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

    [HttpGet]
    [Authorize]
    [ProducesResponseType<List<EnquiryResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<List<EnquiryResponseDto>>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<List<EnquiryResponseDto>>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> List()
    {
        return Ok(await _service.GetListAsync());
    }

    [HttpGet("{id:int}")]
    [Authorize]
    [ProducesResponseType<List<EnquiryResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<List<EnquiryResponseDto>>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<List<EnquiryResponseDto>>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<List<EnquiryResponseDto>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Single(int id)
    {
        try
        {
            return Ok(await _service.GetSingleAsync(id));
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
    }

    [HttpGet("incompletedCount")]
    [Authorize]
    [ProducesResponseType<List<EnquiryResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<List<EnquiryResponseDto>>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<List<EnquiryResponseDto>>(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> IncompletedCount()
    {
        return Ok(await _service.GetIncompletedCountAsync());
    }

    [HttpPost]
    [ProducesResponseType<List<EnquiryResponseDto>>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] EnquiryCreateRequestDto requestDto)
    {
        requestDto.TransformValues();
        ValidationResult validationResult = _validator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        int createdId = await _service.CreateAsync(requestDto);
        return CreatedAtAction("Single", new { id = createdId }, createdId);
    }

    [HttpPost("{id:int}/markAsCompleted")]
    [Authorize]
    [ProducesResponseType<List<EnquiryResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<List<EnquiryResponseDto>>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<List<EnquiryResponseDto>>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<List<EnquiryResponseDto>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsCompleted(int id)
    {
        try
        {
            await _service.MarkAsCompletedAsync(id);
            return Ok();
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
    }
}