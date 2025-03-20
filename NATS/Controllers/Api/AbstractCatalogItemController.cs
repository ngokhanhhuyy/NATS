namespace NATS.Controllers.Api;

public class AbstractCatalogItemController : Controller
{
    private readonly CatalogItemType _type;
    private readonly ICatalogItemService _service;
    private readonly IValidator<CatalogItemUpsertRequestDto> _upsertValidator;

    protected AbstractCatalogItemController(
            CatalogItemType type,
            ICatalogItemService service,
            IValidator<CatalogItemUpsertRequestDto> validator)
    {
        _type = type;
        _service = service;
        _upsertValidator = validator;
    }

    [HttpGet]
    [ProducesResponseType<List<CatalogItemBasicResponseDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> List()
    {
        return Ok(await _service.GetListAsync(_type));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<CatalogItemDetailResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Detail(int id)
    {
        try
        {
            return Ok(await _service.GetDetailAsync(_type, id));
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ValidationProblem(ModelState));
        }
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(CatalogItemUpsertRequestDto requestDto)
    {
        requestDto.TransformValues();
        ValidationResult validationResult = _upsertValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ValidationProblem(ModelState));
        }

        try
        {
            int createdId = await _service.CreateAsync(requestDto);
            string detailUrl = Url.Action("Detail", new { id = createdId });
            return Created(detailUrl, createdId);
        }
        catch (ConcurrencyException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return Conflict(ValidationProblem(ModelState));
        }
    }

    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(int id, CatalogItemUpsertRequestDto requestDto)
    {
        requestDto.TransformValues();
        ValidationResult validationResult = _upsertValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ValidationProblem(ModelState));
        }

        try
        {
            await _service.UpdateAsync(id, requestDto);
            return Ok();
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ValidationProblem(ModelState));
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return UnprocessableEntity(ValidationProblem(ModelState));
        }
        catch (ConcurrencyException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return Conflict(ValidationProblem(ModelState));
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
            return NotFound(ValidationProblem(ModelState));
        }
        catch (ConcurrencyException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return Conflict(ValidationProblem(ModelState));
        }
    }
}