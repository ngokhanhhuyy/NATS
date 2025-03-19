namespace NATS.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class SliderItemController : ControllerBase
{
    private readonly ISliderItemService _service;
    private readonly IValidator<SliderItemUpsertRequestDto> _validator;

    public SliderItemController(
            ISliderItemService service,
            IValidator<SliderItemUpsertRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> List()
    {
        return Ok(await _service.GetListAsync());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
}
