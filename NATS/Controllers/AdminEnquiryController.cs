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
        ServiceResult<List<EnquiryResponseDto>> serviceResult;
        serviceResult = await _service.GetListAsync();
        
        // Initialize view model and map data from the response dtos.
        EnquiryListViewModel model = new EnquiryListViewModel
        {
            Items = serviceResult.ResponseDto
                .Select(e => new EnquiryViewModel
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                    Content = e.Content,
                    ReceivedDateTime = e.ReceivedDateTime,
                    IsCompleted = e.IsCompleted
                }).ToList()
        };
        
        return View("~/Views/Admin/Enquiry/EnquiryList.cshtml", model);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Detail(int id)
    {
        // Fetch the detail of the enquiry with given id.
        ServiceResult<EnquiryResponseDto> serviceResult;
        serviceResult = await _service.GetAsync(id);
        
        // Ensure the enquiry exists.
        if (!serviceResult.Succeeded)
        {
            return NotFound();
        }
        
        // Initialize view model and map data from the response dto.
        EnquiryViewModel model = new EnquiryViewModel
        {
            Id = serviceResult.ResponseDto.Id,
            FullName = serviceResult.ResponseDto.FullName,
            Email = serviceResult.ResponseDto.Email,
            PhoneNumber = serviceResult.ResponseDto.PhoneNumber,
            Content = serviceResult.ResponseDto.Content,
            ReceivedDateTime = serviceResult.ResponseDto.ReceivedDateTime,
            IsCompleted = serviceResult.ResponseDto.IsCompleted
        };
        
        return View("~/Views/Admin/Enquiry/Enquiry.cshtml", model);
    }
    
    [HttpPost("{id:int}/danh-dau-da-hoan-thanh")]
    public async Task<IActionResult> MarkingAsCompleted(int id)
    {
        // Mark enquiry with given id as completed.
        ServiceResult<int> serviceResult;
        serviceResult = await _service.MarkAsCompletedAsync(id);
        
        // Ensure the enquiry exists.
        if (!serviceResult.Succeeded)
        {
            return NotFound();
        }
        
        // Notify clients that the operation has been performed successfully.
        return Ok();
    }
}