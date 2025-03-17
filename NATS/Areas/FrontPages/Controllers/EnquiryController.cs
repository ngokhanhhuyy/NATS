using NATS.FrontPages.Models;

namespace NATS.FrontPages.Controllers;

[Area("FrontPages")]
[Route("/lien-he")]
public class EnquiryController : Controller
{
    private readonly IEnquiryService _enquiryService;
    private readonly IContactService _contactService;

    public EnquiryController(IEnquiryService enquiryService, IContactService contactService)
    {
        _enquiryService = enquiryService;
        _contactService = contactService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<ContactResponseDto> contactResponseDtos = await _contactService.GetListAsync();
        EnquiryViewModel model = new EnquiryViewModel(contactResponseDtos);

        return View(model);
    }
}