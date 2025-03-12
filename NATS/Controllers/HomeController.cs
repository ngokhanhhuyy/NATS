namespace NATS.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly ISliderItemService _sliderItemService;
    private readonly IAboutUsIntroductionService _aboutUsIntroductionService;
    private readonly IMemberService _memberService;
    private readonly ICertificateService _certificateService;
    private readonly ISummaryItemService _summaryItemService;
    private readonly ICatalogItemService _catalogItemService;
    private readonly IPostService _postService;
    private readonly IEnquiryService _enquiryService;
    private readonly IContactService _contactService;

    public HomeController(
            ISliderItemService sliderItemService,
            IAboutUsIntroductionService aboutUsIntroductionService,
            IMemberService memberService,
            ICertificateService certificateService,
            ISummaryItemService summaryItemService,
            ICatalogItemService catalogItemService,
            IPostService postService,
            IEnquiryService enquiryService,
            IContactService contactService)
    {
        _sliderItemService = sliderItemService;
        _aboutUsIntroductionService = aboutUsIntroductionService;
        _memberService = memberService;
        _certificateService = certificateService;
        _summaryItemService = summaryItemService;
        _catalogItemService = catalogItemService;
        _postService = postService;
        _enquiryService = enquiryService;
        _contactService = contactService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Prepare tasks.
        Task<List<SliderItemResponseDto>> sliderItemsTask;
        sliderItemsTask = _sliderItemService.GetListAsync();

        Task<List<SummaryItemResponseDto>> summaryItemsTask;
        summaryItemsTask = _summaryItemService.GetListAsync();

        Task<List<CatalogItemBasicResponseDto>> coursesTask;
        coursesTask = _catalogItemService.GetListAsync(CatalogItemType.Course);

        Task<List<CatalogItemBasicResponseDto>> servicesTask;
        servicesTask = _catalogItemService.GetListAsync(CatalogItemType.Service);

        Task<List<CatalogItemBasicResponseDto>> productsTask;
        productsTask = _catalogItemService.GetListAsync(CatalogItemType.Product);

        // Await for tasks to be done.
        await Task.WhenAll(
            sliderItemsTask,
            summaryItemsTask,
            coursesTask,
            servicesTask,
            productsTask);

        HomePageModel model = new HomePageModel(
            sliderItemsTask.Result,
            summaryItemsTask.Result,
            coursesTask.Result,
            servicesTask.Result,
            productsTask.Result);
            
        return View("Index", model);
    }

    [HttpGet("ve-chung-toi")]
    public async Task<IActionResult> AboutUs()
    {
        AboutUsIntroductionResponseDto responseDto;
        responseDto = await _aboutUsIntroductionService.GetAsync();
        AboutUsIntroductionDetailModel model = new AboutUsIntroductionDetailModel(responseDto);
        
        return View("AboutUs", model);
    }

    [HttpGet("doi-ngu")]
    public async Task<IActionResult> Members()
    {
        
        Task<List<MemberResponseDto>> membersTask = _memberService.GetListAsync();
        Task<List<CertificateResponseDto>> certificatesTask;
        certificatesTask = _certificateService.GetListAsync();

        await Task.WhenAll(membersTask, certificatesTask);

        MembersAndCertificatesModel model = new MembersAndCertificatesModel(
            membersTask.Result,
            certificatesTask.Result);

        return View("TeamMembers", model);
    }

    [HttpGet("gioi-thieu")]
    public async Task<IActionResult> SummaryList()
    {
        List<SummaryItemResponseDto> responseDtos = await _summaryItemService.GetListAsync();
        List<SummaryItemDetailModel> model = responseDtos
            .Select(dto => new SummaryItemDetailModel(dto))
            .ToList();

        return View("IntroductionItemList", model);
    }

    [HttpGet("khoa-hoc")]
    public async Task<IActionResult> CourseList()
    {
        List<CatalogItemBasicResponseDto> responseDtos;
        responseDtos = await _catalogItemService.GetListAsync(CatalogItemType.Course);

        List<CatalogItemBasicModel> model = responseDtos
            .Select(dto => new CatalogItemBasicModel(dto))
            .ToList();

        return View("CourseDetailList", model);
    }

    [HttpGet("dich-vu")]
    public async Task<IActionResult> ServiceList()
    {
        List<CatalogItemBasicResponseDto> responseDtos;
        responseDtos = await _catalogItemService.GetListAsync(CatalogItemType.Service);

        List<CatalogItemBasicModel> model = responseDtos
            .Select(dto => new CatalogItemBasicModel(dto))
            .ToList();

        return View("BusinessServiceDetailList", model);
    }

    [HttpGet("san-pham")]
    public async Task<IActionResult> ProductList()
    {
        List<CatalogItemBasicResponseDto> responseDtos;
        responseDtos = await _catalogItemService.GetListAsync(CatalogItemType.Product);

        List<CatalogItemBasicModel> model = responseDtos
            .Select(dto => new CatalogItemBasicModel(dto))
            .ToList();

        return View("ProductDetailList", model);
    }
    
    [HttpGet("lien-he")]
    public async Task<IActionResult> Contact()
    {
        List<ContactResponseDto> responseDtos = await _contactService.GetListAsync();
        List<ContactDetailModel> model = responseDtos
            .Select(dto => new ContactDetailModel(dto))
            .ToList();
        
        // Return the view.
        return View("~/Views/Home/Contact.cshtml", model);
    }
    
    // [HttpPost("lien-he")]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Contact(ContactListModel listModel)
    // {
    //     // Map enquiry data from view model to request dto.
    //     EnquiryUpsertRequestDto requestDto = new EnquiryUpsertRequestDto
    //     {
    //         FullName = listModel.Enquiry?.FullName,
    //         PhoneNumber = listModel.Enquiry?.PhoneNumber,
    //         Email = listModel.Enquiry?.Email,
    //         Content = listModel.Enquiry?.Content
    //     };
        
    //     // Perform creating operation.
    //     ServiceResult<int> enquiryServiceResult;
    //     enquiryServiceResult = await _enquiryService.CreateAsync(requestDto);
    //     if (!enquiryServiceResult.Succeeded)
    //     {
    //         // Fetch contact info data to display the page again.
    //         ServiceResult<ContactResponseDto> contactInfoServiceResult;
    //         contactInfoServiceResult = await _contactService.GetListAsync();
    //         listModel.ContactInfo = new ContactModel
    //         {
    //             PhoneNumber = contactInfoServiceResult.ResponseDto.PhoneNumber,
    //             ZaloNumber = contactInfoServiceResult.ResponseDto.ZaloNumber,
    //             Email = contactInfoServiceResult.ResponseDto.Email,
    //             Address = contactInfoServiceResult.ResponseDto.Address
    //         };
            
    //         // Add error message returned from the service call to model state for displaying.
    //         ModelState.Clear();
    //         enquiryServiceResult.Errors.ForEach(error =>
    //         {
    //             ModelState.AddModelError(
    //                 nameof(listModel.Enquiry) + "." + error.PropertyName,
    //                 error.ErrorMessage);
    //         });
    //         return View("~/Views/Home/Contact.cshtml", listModel);
    //     }
        
    //     (string, string, string) saveSuccessModel = (
    //         "Câu hỏi được gửi thành công",
    //         "Cảm ơn bạn đã đặt câu hỏi cho chúng tôi. " + Environment.NewLine +
    //         "Chúng tôi sẽ liên hệ với bạn theo thông tin đã được cung cấp theo thời gian sớm nhất.",
    //         Url.Action("Index", "Home"));

    //     return RedirectToAction("Success", "Home", saveSuccessModel);
    // }
    
    // [HttpGet("bai-viet/trang-{page:int}")]
    // public async Task<IActionResult> PostList(int page)
    // {
    //     // Fetch list of posts.
    //     ServiceResult<PostBasicListResponseDto> serviceResult;
    //     serviceResult = await _postService.GetBasicListAsync(page);
        
    //     // Initialize view model and map data from response dto to its properties.
    //     PostBasicListViewModel model = new PostBasicListViewModel
    //     {
    //         Items = serviceResult.ResponseDto.Items
    //             .Select(p => new PostBasicViewModel
    //             {
    //                 Id = p.Id,
    //                 Title = p.Title,
    //                 NormalizedTitle = p.NormalizedTitle,
    //                 ThumbnailUrl = p.ThumbnailUrl,
    //                 Content = p.Content,
    //                 CreatedDateTime = p.CreatedDateTime,
    //                 IsPublished = p.IsPublished,
    //                 IsPinned = p.IsPinned,
    //                 Views = p.Views
    //             }).ToList(),
    //         Page = page,
    //         PageCount = serviceResult.ResponseDto.PageCount
    //     };
        
    //     // Return to view.
    //     return View("PostList", model);
    // }

    // [HttpGet("bai-viet/{normalizedTitle}")]
    // public async Task<IActionResult> Post(string normalizedTitle)
    // {
    //     ServiceResult<PostDetailResponseDto> serviceResult;
    //     serviceResult = await _postService.GetDetailAsync(normalizedTitle, true);
    //     if (!serviceResult.Succeeded)
    //     {
    //         return NotFound();
    //     }

    //     PostDetailViewModel model = new PostDetailViewModel
    //     {
    //         Id = serviceResult.ResponseDto.Id,
    //         Title = serviceResult.ResponseDto.Title,
    //         ThumbnailUrl = serviceResult.ResponseDto.ThumbnailUrl,
    //         Content = serviceResult.ResponseDto.Content,
    //         CreatedDateTime = serviceResult.ResponseDto.CreatedDateTime,
    //         UpdatedDateTime = serviceResult.ResponseDto.UpdatedDateTime,
    //         IsPinned = serviceResult.ResponseDto.IsPinned,
    //         IsPublished = serviceResult.ResponseDto.IsPublished,
    //         Views = serviceResult.ResponseDto.Views,
    //         User = new UserBasicViewModel
    //         {
    //             Id = serviceResult.ResponseDto.User.Id,
    //             UserName = serviceResult.ResponseDto.User.UserName,
    //             Role = new RoleViewModel
    //             {
    //                 Id = serviceResult.ResponseDto.User.Role.Id,
    //                 Name = serviceResult.ResponseDto.User.Role.Name,
    //                 DisplayName = serviceResult.ResponseDto.User.Role.DisplayName
    //             }
    //         }
    //     };

    //     return View("Post", model);
    // }
    
    [HttpGet("thanh-cong")]
    public IActionResult Success((string Title, string Content, string RedirectUrl) model)
    {
        return View("SaveSuccess", model);
    }

    [HttpGet("bao-tri")]
    public IActionResult UnderMaintainance()
    {
        return View("UnderMaintainance");
    }
    
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok();
    }
}
