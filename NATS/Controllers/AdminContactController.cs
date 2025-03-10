namespace NATS.Controllers;

public class AdminContactController : Controller
{
    private readonly IContactService _service;
    private readonly IValidator<ContactUpsertRequestDto> _validator;

    public AdminContactController(
            IContactService service,
            IValidator<ContactUpsertRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        List<ContactResponseDto> responseDtos = await _service.GetListAsync();
        List<ContactModel> model = responseDtos
            .Select(dto => new ContactModel(dto))
            .ToList();

        return View("", model);
    }
}