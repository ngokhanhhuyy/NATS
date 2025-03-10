namespace NATS.Controllers;

[Route("quan-tri/cau-hoi")]
public class AdminEnquiryController : Controller
{
    private readonly IEnquiryService _service;
    
    public AdminEnquiryController(IEnquiryService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> List()
    {
        // Fetch a list of all enquiries.
        List<EnquiryResponseDto> responseDtos = await _service.GetListAsync();
        
        // Initialize model and map data from the response dtos.
        List<EnquiryModel> model = responseDtos
            .Select(dto => new EnquiryModel(dto))
            .ToList();
        
        return View("~/Views/Admin/Enquiry/EnquiryList.cshtml", model);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Detail(int id)
    {
        try
        {
            EnquiryResponseDto responseDto = await _service.GetSingleAsync(id);
            EnquiryModel model = new EnquiryModel(responseDto);
            return View("~/Views/Admin/Enquiry/Enquiry.cshtml", model);
        }
        catch (ResourceNotFoundException)
        {
            return RedirectToAction("List", "AdminEnquiry");
        }
    }
    
    [HttpPost("{id:int}/danh-dau-da-hoan-thanh")]
    public async Task<IActionResult> MarkingAsCompleted(int id)
    {
        try
        {
            // Mark enquiry with given id as completed.
            await _service.MarkAsCompletedAsync(id);
        }
        catch (ResourceNotFoundException)
        {
            return RedirectToAction("List", "AdminEnquiry");
        }

        return RedirectToAction("Detail", "AdminEnquiry");
    }
}