namespace NATS.Controllers;

[Route("/api/[controller]")]
public class SliderItemController : ControllerBase
{
    private readonly ISliderItemService _service;

    public SliderItemController(ISliderItemService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> List()
    {
        return Ok(await _service.GetListAsync());
    }
}
