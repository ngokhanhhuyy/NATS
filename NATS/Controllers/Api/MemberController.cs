namespace NATS.Controllers.Api;

[Route("/Api/[controller]")]
public class MemberController : ControllerBase
{
    private readonly IMemberService _service;
    private readonly IValidator<MemberUpsertRequestDto> _validator;

    public MemberController(
            IMemberService service,
            IValidator<MemberUpsertRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    [HttpGet]
    [ProducesResponseType<List<MemberResponseDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> List()
    {
        return Ok(await _service.GetListAsync());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<List<MemberResponseDto>>(StatusCodes.Status200OK)]
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

    [HttpPost]
    [Authorize]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] MemberUpsertRequestDto requestDto)
    {
        requestDto.TransformValues();
        ValidationResult validationResult = _validator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        int createdId = await _service.CreateAsync(requestDto);
        return CreatedAtAction("Single", createdId);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
            int id,
            [FromBody] MemberUpsertRequestDto requestDto)
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

    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
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