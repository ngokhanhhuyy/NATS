namespace NATS.Controllers.Api;

[Route("/Api/[controller]")]
public class CatalogItemController : Controller
{
    private readonly ICatalogItemService _service;
    private readonly IValidator<CatalogItemListRequestDto> _listValidator;
    private readonly IValidator<CatalogItemUpsertRequestDto> _upsertValidator;

    public CatalogItemController(
            ICatalogItemService service,
            IValidator<CatalogItemListRequestDto> listValidator,
            IValidator<CatalogItemUpsertRequestDto> upsertValidator)
    {
        _service = service;
        _listValidator = listValidator;
        _upsertValidator = upsertValidator;
    }

    [HttpGet]
    [ProducesResponseType<List<CatalogItemBasicResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> List(CatalogItemListRequestDto requestDto)
    {
        ValidationResult validationResult = _listValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        return Ok(await _service.GetListAsync(requestDto));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<CatalogItemDetailResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Detail(int id)
    {
        try
        {
            return Ok(await _service.GetDetailAsync(id));
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType<int>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CatalogItemUpsertRequestDto requestDto)
    {
        requestDto.TransformValues();
        ValidationResult validationResult = _upsertValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        try
        {
            int createdId = await _service.CreateAsync(requestDto);
            string detailUrl = Url.Action("Detail", new { id = createdId });
            return Created(detailUrl, createdId);
        }
        catch (ConcurrencyException)
        {
            return Conflict();
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
    public async Task<IActionResult> Update(
            int id,
            [FromBody] CatalogItemUpsertRequestDto requestDto)
    {
        requestDto.TransformValues();
        ValidationResult validationResult = _upsertValidator.Validate(requestDto);
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
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return UnprocessableEntity(ModelState);
        }
        catch (ConcurrencyException)
        {
            return Conflict();
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
            return NotFound(ModelState);
        }
        catch (ConcurrencyException)
        {
            return Conflict();
        }
    }
}