namespace NATS.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly IHomePageSliderItemService _homePageSliderItemService;
    private readonly IAboutUsIntroductionService _aboutUsIntroductionService;
    private readonly ITeamMembersService _teamMembersService;
    private readonly IBusinessCertificateService _businessCertificateService;
    private readonly IIntroductionItemService _introductionItemService;
    private readonly ICourseService _courseService;
    private readonly IBusinessServiceService _businessServiceService;
    private readonly IProductService _productService;
    private readonly IPostService _postService;
    private readonly IEnquiryService _enquiryService;
    private readonly IContactInfoService _contactInfoService;

    public HomeController(
            IHomePageSliderItemService homePageSliderItemService,
            IAboutUsIntroductionService aboutUsIntroductionService,
            ITeamMembersService teamMemberService,
            IBusinessCertificateService businessCertificateService,
            IIntroductionItemService introductionItemService,
            ICourseService courseService,
            IBusinessServiceService businessServiceService,
            IProductService productService,
            IPostService postService,
            IEnquiryService enquiryService,
            IContactInfoService contactInfoService)
    {
        _homePageSliderItemService = homePageSliderItemService;
        _aboutUsIntroductionService = aboutUsIntroductionService;
        _teamMembersService = teamMemberService;
        _businessCertificateService = businessCertificateService;
        _introductionItemService = introductionItemService;
        _courseService = courseService;
        _businessServiceService = businessServiceService;
        _productService = productService;
        _postService = postService;
        _enquiryService = enquiryService;
        _contactInfoService = contactInfoService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Fetch homepage slider item list
        ServiceResult<List<HomePageSliderItemResponseDto>> _homePageSliderItemServiceResult;
        _homePageSliderItemServiceResult = await _homePageSliderItemService.GetListAsync();

        // Fetch course introduction item list
        ServiceResult<List<IntroductionItemResponseDto>> _introductionItemServiceResult;
        _introductionItemServiceResult = await _introductionItemService.GetListAsync();
        
        // Fetch course list
        ServiceResult<List<CourseBasicResponseDto>> courseServiceResult;
        courseServiceResult = await _courseService.GetBasicListAsync();
        
        // Fetch service list
        ServiceResult<List<BusinessServiceBasicResponseDto>> businessServiceServiceResult;
        businessServiceServiceResult = await _businessServiceService.GetBasicListAsync();

        // Fetch product list
        ServiceResult<List<ProductBasicResponseDto>> productServiceResult;
        productServiceResult = await _productService.GetBasicListAsync();

        HomePageViewModel model = new HomePageViewModel
        {
            HomePageSliderItems = new HomePageSliderItemListViewModel
            {
                Items = _homePageSliderItemServiceResult.ResponseDto
                    .Select(sliderItem => new HomePageSliderItemViewModel
                    {
                        Id = sliderItem.Id,
                        Title = sliderItem.Title,
                        PhotoUrl = sliderItem.PhotoUrl
                    }).ToList()
            },
            IntroductionItems = new IntroductionItemListViewModel
            {
                Items = _introductionItemServiceResult.ResponseDto
                    .Select(ii => new IntroductionItemViewModel
                    {
                        Id = ii.Id,
                        Name = ii.Name,
                        Summary = ii.Summary,
                        Content = ii.Content,
                        ThumbnailUrl = ii.ThumbnailUrl
                    }).ToList()
            },
            Courses = new CourseBasicListViewModel
            {
                Items = courseServiceResult.ResponseDto
                    .Select(bs => new CourseBasicViewModel
                    {
                        Id = bs.Id,
                        Name = bs.Name,
                        Summary = bs.Summary,
                        ThumbnailUrl = bs.ThumbnailUrl
                    }).ToList()
            },
            BusinessServices = new BusinessServiceBasicListViewModel
            {
                Items = businessServiceServiceResult.ResponseDto
                    .Select(bs => new BusinessServiceBasicViewModel
                    {
                        Id = bs.Id,
                        Name = bs.Name,
                        Summary = bs.Summary,
                        ThumbnailUrl = bs.ThumbnailUrl
                    }).ToList()
            },
            Products = new ProductBasicListViewModel
            {
                Items = productServiceResult.ResponseDto
                    .Select(bs => new ProductBasicViewModel
                    {
                        Id = bs.Id,
                        Name = bs.Name,
                        Summary = bs.Summary,
                        ThumbnailUrl = bs.ThumbnailUrl
                    }).ToList()
            }
        };
        return View("Index", model);
    }

    [HttpGet("ve-chung-toi")]
    public async Task<IActionResult> AboutUs()
    {
        ServiceResult<AboutUsIntroductionResponseDto> serviceResult;
        serviceResult = await _aboutUsIntroductionService.GetAsync();
        AboutUsIntroductionViewModel model = new AboutUsIntroductionViewModel
        {
            MainPhotoUrl = serviceResult.ResponseDto.MainPhotoUrl,
            MainQuoteContent = serviceResult.ResponseDto.MainQuoteContent,
            AboutUsContent = serviceResult.ResponseDto.AboutUsContent,
            WhyChooseUsContent = serviceResult.ResponseDto.WhyChooseUsContent,
            OurDifferenceContent = serviceResult.ResponseDto.OurDifferenceContent,
            OurCultureContent = serviceResult.ResponseDto.OurCultureContent
        };
        return View("AboutUs", model);
    }

    [HttpGet("doi-ngu")]
    public async Task<IActionResult> TeamMembers()
    {
        ServiceResult<List<TeamMemberResponseDto>> teamMembersServiceResult;
        teamMembersServiceResult = await _teamMembersService.GetListAsync();
        
        ServiceResult<List<BusinessCertificateResponseDto>> businessCertificateServiceResult;
        businessCertificateServiceResult = await _businessCertificateService.GetListAsync();

        TeamMemberListViewModel model = new TeamMemberListViewModel
        {
            TeamMembers = teamMembersServiceResult.ResponseDto
                .Select(dto => new TeamMemberViewModel
                {
                    Id = dto.Id,
                    FullName = dto.FullName,
                    RoleName = dto.RoleName,
                    Description = dto.Description,
                    PhotoUrl = dto.PhotoUrl
                }).ToList(),
            BusinessCertificates = businessCertificateServiceResult.ResponseDto
                .Select(dto => new BusinessCertificateViewModel
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    PhotoUrl = dto.PhotoUrl
                }).ToList()
        };
        return View("TeamMembers", model);
    }

    [HttpGet("gioi-thieu")]
    public async Task<IActionResult> IntroductionList()
    {
        ServiceResult<List<IntroductionItemResponseDto>> serviceResult;
        serviceResult = await _introductionItemService.GetListAsync();
        IntroductionItemListViewModel model = new IntroductionItemListViewModel
        {
            Items = serviceResult.ResponseDto
                .Select(ii => new IntroductionItemViewModel
                    {
                        Id = ii.Id,
                        Name = ii.Name,
                        Summary = ii.Summary,
                        Content = ii.Content,
                        ThumbnailUrl = ii.ThumbnailUrl,
                    }).ToList(),
            ItemType = ItemType.Courses
        };
        return View("IntroductionItemList", model);
    }

    [HttpGet("khoa-hoc")]
    public async Task<IActionResult> CourseList()
    {
        ServiceResult<List<CourseDetailResponseDto>> serviceResult;
        serviceResult = await _courseService.GetDetailListAsync();

        CourseDetailListViewModel model = new CourseDetailListViewModel
        {
            Items = serviceResult.ResponseDto
                .Select(c => new CourseDetailViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Summary = c.Summary,
                    Detail = c.Detail,
                    ThumbnailUrl = c.ThumbnailUrl,
                    Sections = c.Sections?
                        .Select(cs => new CourseSectionViewModel
                        {
                            Id = cs.Id,
                            Content = cs.Content
                        }).ToList(),
                    Photos = c.Photos?
                        .Select(cp => new CoursePhotoViewModel
                        {
                            Id = cp.Id,
                            Url = cp.Url
                        }).ToList()
                }).ToList()
        };

        return View("CourseDetailList", model);
    }

    [HttpGet("dich-vu")]
    public async Task<IActionResult> BusinessServiceList()
    {
        ServiceResult<List<BusinessServiceDetailResponseDto>> serviceResult;
        serviceResult = await _businessServiceService.GetDetailListAsync();

        BusinessServiceDetailListViewModel model = new BusinessServiceDetailListViewModel
        {
            Items = serviceResult.ResponseDto
                .Select(bs => new BusinessServiceDetailViewModel
                {
                    Id = bs.Id,
                    Name = bs.Name,
                    Summary = bs.Summary,
                    Detail = bs.Detail,
                    ThumbnailUrl = bs.ThumbnailUrl,
                    Features = bs.Features?.Select(bsf => new BusinessServiceFeatureViewModel
                    {
                        Id = bsf.Id,
                        Content = bsf.Content
                    }).ToList(),
                    Photos = bs.Photos?.Select(bsp => new BusinessServicePhotoViewModel
                    {
                        Id = bsp.Id,
                        Url = bsp.Url
                    }).ToList()
                }).ToList()
        };

        return View("BusinessServiceDetailList", model);
    }

    [HttpGet("san-pham")]
    public async Task<IActionResult> ProductList()
    {
        ServiceResult<List<ProductDetailResponseDto>> serviceResult;
        serviceResult = await _productService.GetDetailListAsync();

        ProductDetailListViewModel model = new ProductDetailListViewModel
        {
            Items = serviceResult.ResponseDto
                .Select(bs => new ProductDetailViewModel
                {
                    Id = bs.Id,
                    Name = bs.Name,
                    Summary = bs.Summary,
                    Detail = bs.Detail,
                    ThumbnailUrl = bs.ThumbnailUrl,
                    Features = bs.Features?.Select(pf => new ProductFeatureViewModel
                    {
                        Id = pf.Id,
                        Content = pf.Content
                    }).ToList(),
                    Photos = bs.Photos?.Select(pp => new ProductPhotoViewModel
                    {
                        Id = pp.Id,
                        Url = pp.Url
                    }).ToList()
                }).ToList()
        };

        return View("ProductDetailList", model);
    }
    
    [HttpGet("lien-he")]
    public async Task<IActionResult> Contact()
    {
        ServiceResult<ContactInfoResponseDto> contactInfoServiceResult;
        contactInfoServiceResult = await _contactInfoService.GetAsync();
        
        // Initialize view model.
        ContactViewModel model = new ContactViewModel
        {
            ContactInfo = new ContactInfoViewModel
            {
                PhoneNumber = contactInfoServiceResult.ResponseDto.PhoneNumber,
                ZaloNumber = contactInfoServiceResult.ResponseDto.ZaloNumber,
                Email = contactInfoServiceResult.ResponseDto.Email,
                Address = contactInfoServiceResult.ResponseDto.Address
            }
        };
        
        // Return the view.
        return View("~/Views/Home/Contact.cshtml", model);
    }
    
    [HttpPost("lien-he")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact(ContactViewModel model)
    {
        // Map enquiry data from view model to request dto.
        EnquiryRequestDto requestDto = new EnquiryRequestDto
        {
            FullName = model.Enquiry?.FullName,
            PhoneNumber = model.Enquiry?.PhoneNumber,
            Email = model.Enquiry?.Email,
            Content = model.Enquiry?.Content
        };
        
        // Perform creating operation.
        ServiceResult<int> enquiryServiceResult;
        enquiryServiceResult = await _enquiryService.CreateAsync(requestDto);
        if (!enquiryServiceResult.Succeeded)
        {
            // Fetch contact info data to display the page again.
            ServiceResult<ContactInfoResponseDto> contactInfoServiceResult;
            contactInfoServiceResult = await _contactInfoService.GetAsync();
            model.ContactInfo = new ContactInfoViewModel
            {
                PhoneNumber = contactInfoServiceResult.ResponseDto.PhoneNumber,
                ZaloNumber = contactInfoServiceResult.ResponseDto.ZaloNumber,
                Email = contactInfoServiceResult.ResponseDto.Email,
                Address = contactInfoServiceResult.ResponseDto.Address
            };
            
            // Add error message returned from the service call to model state for displaying.
            ModelState.Clear();
            enquiryServiceResult.Errors.ForEach(error =>
            {
                ModelState.AddModelError(
                    nameof(model.Enquiry) + "." + error.PropertyName,
                    error.ErrorMessage);
            });
            return View("~/Views/Home/Contact.cshtml", model);
        }
        
        (string, string, string) saveSuccessModel = (
            "Câu hỏi được gửi thành công",
            "Cảm ơn bạn đã đặt câu hỏi cho chúng tôi. " + Environment.NewLine +
            "Chúng tôi sẽ liên hệ với bạn theo thông tin đã được cung cấp theo thời gian sớm nhất.",
            Url.Action("Index", "Home"));
        return RedirectToAction("Success", "Home", saveSuccessModel);
    }
    
    [HttpGet("bai-viet/trang-{page:int}")]
    public async Task<IActionResult> PostList(int page)
    {
        // Fetch list of posts.
        ServiceResult<PostBasicListResponseDto> serviceResult;
        serviceResult = await _postService.GetBasicListAsync(page);
        
        // Initialize view model and map data from response dto to its properties.
        PostBasicListViewModel model = new PostBasicListViewModel
        {
            Items = serviceResult.ResponseDto.Items
                .Select(p => new PostBasicViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    NormalizedTitle = p.NormalizedTitle,
                    ThumbnailUrl = p.ThumbnailUrl,
                    Content = p.Content,
                    CreatedDateTime = p.CreatedDateTime,
                    IsPublished = p.IsPublished,
                    IsPinned = p.IsPinned,
                    Views = p.Views
                }).ToList(),
            Page = page,
            PageCount = serviceResult.ResponseDto.PageCount
        };
        
        // Return to view.
        return View("PostList", model);
    }

    [HttpGet("bai-viet/{normalizedTitle}")]
    public async Task<IActionResult> Post(string normalizedTitle)
    {
        ServiceResult<PostDetailResponseDto> serviceResult;
        serviceResult = await _postService.GetDetailAsync(normalizedTitle, true);
        if (!serviceResult.Succeeded)
        {
            return NotFound();
        }

        PostDetailViewModel model = new PostDetailViewModel
        {
            Id = serviceResult.ResponseDto.Id,
            Title = serviceResult.ResponseDto.Title,
            ThumbnailUrl = serviceResult.ResponseDto.ThumbnailUrl,
            Content = serviceResult.ResponseDto.Content,
            CreatedDateTime = serviceResult.ResponseDto.CreatedDateTime,
            UpdatedDateTime = serviceResult.ResponseDto.UpdatedDateTime,
            IsPinned = serviceResult.ResponseDto.IsPinned,
            IsPublished = serviceResult.ResponseDto.IsPublished,
            Views = serviceResult.ResponseDto.Views,
            User = new UserBasicViewModel
            {
                Id = serviceResult.ResponseDto.User.Id,
                UserName = serviceResult.ResponseDto.User.UserName,
                Role = new RoleViewModel
                {
                    Id = serviceResult.ResponseDto.User.Role.Id,
                    Name = serviceResult.ResponseDto.User.Role.Name,
                    DisplayName = serviceResult.ResponseDto.User.Role.DisplayName
                }
            }
        };

        return View("Post", model);
    }
    
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
