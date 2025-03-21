namespace NATS.Controllers.Api;

[Route("/Api/[controller]")]
public class SummaryItemController : ControllerBase
{
    private readonly ISummaryItemService _service;
    private readonly IValidator<SummaryItemUpdateRequestDto> _validator;

    public SummaryItemController(
            ISummaryItemService service,
            IValidator<SummaryItemUpdateRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    [HttpGet]
    [ProducesResponseType<List<SummaryItemResponseDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> List()
    {
        return Ok(await _service.GetListAsync());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<List<SummaryItemResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    [HttpPut("{id:int}")]
    [ProducesResponseType<List<SummaryItemResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, SummaryItemUpdateRequestDto requestDto)
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
            await _service.UpdateAsync(id, requestDto);
            return Ok();
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
        catch (ConcurrencyException)
        {
            return Conflict();
        }
    }
}