namespace NATS.Controllers;

[Route("/quan-tri")]
[Authorize]
public class AdminController : Controller
{
    private readonly IAboutUsIntroductionService _aboutUsIntroductionService;
    private readonly ITeamMembersService _teamMembersService;
    private readonly IBusinessCertificateService _businessCertificateService;
    private readonly IGeneralSettingsService _generalSettingsService;
    private readonly IHomePageSliderItemService _homePageSliderItemService;
    private readonly IIntroductionItemService _introductionItemService;
    private readonly IPostService _postService;
    private readonly IEnquiryService _enquiryService;
    private readonly IContactInfoService _contactInfoService;
    private readonly ITrafficService _trafficService;
    private readonly IUserService _userService;

    public AdminController(
            IAboutUsIntroductionService aboutUsIntroductionService,
            ITeamMembersService teamMembersService,
            IBusinessCertificateService businessCertificateService,
            IGeneralSettingsService generalSettingsService,
            IHomePageSliderItemService homePageSliderItemService,
            IIntroductionItemService introductionItemService,
            IPostService postService,
            IEnquiryService enquiryService,
            IContactInfoService contactInfoService,
            ITrafficService trafficService,
            IUserService userService)
    {
        _aboutUsIntroductionService = aboutUsIntroductionService;
        _teamMembersService = teamMembersService;
        _businessCertificateService = businessCertificateService;
        _generalSettingsService = generalSettingsService;
        _homePageSliderItemService = homePageSliderItemService;
        _introductionItemService = introductionItemService;
        _postService = postService;
        _enquiryService = enquiryService;
        _contactInfoService = contactInfoService;
        _trafficService = trafficService;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        // Get dates traffic statistics.
        ServiceResult<List<TrafficStatisticsByDateResponseDto>> datesTrafficServiceResult;
        datesTrafficServiceResult = await _trafficService.GetStatisticsByDateRangeAsync(7);

        // Get hours traffic statistics.
        ServiceResult<List<TrafficStatisticsByHourRangeResponseDto>> hoursTrafficServiceResult;
        hoursTrafficServiceResult = await _trafficService.GetStatisticsByHourRangeAsync(7);

        // Get devices traffic statistics.
        ServiceResult<List<TrafficStatisticsByDeviceResponseDto>> devicesTrafficServiceResult;
        devicesTrafficServiceResult = await _trafficService.GetStatisticsByDeviceAsync(7);

        // Get the number of incompleted enquiries.
        ServiceResult<int> enquiryServiceResult;
        enquiryServiceResult = await _enquiryService.GetIncompletedCountAsync();
        
        // Get the number of users.
        ServiceResult<int> userServiceResult;
        userServiceResult = await _userService.GetCountAsync();
        
        DashboardViewModel model = new DashboardViewModel
        {
            AccessCount = datesTrafficServiceResult.ResponseDto
                .LastOrDefault()?.AccessCount ?? 0,
            GuessCount = datesTrafficServiceResult.ResponseDto
                .LastOrDefault()?.GuessCount ?? 0,
            EnquiryCount = enquiryServiceResult.ResponseDto,
            UserCount = userServiceResult.ResponseDto,
            TrafficStatisticsByDates = new TrafficStatisticsByDateListViewModel
            {
                Items = datesTrafficServiceResult.ResponseDto
                    .Select(td => new TrafficStatisticsByDateViewModel
                    {
                        RecordedDate = td.RecordedDate,
                        AccessCount = td.AccessCount,
                        GuessCount = td.GuessCount
                    }).ToList()
            },
            TrafficStatisticsByHourRanges = new TrafficStatisticsByHourRangeListViewModel
            {
                Items = hoursTrafficServiceResult.ResponseDto
                    .Select(th => new TrafficStatisticsByHourRangeViewModel
                    {
                        Name = th.Name,
                        FromTime = th.FromTime,
                        ToTime = th.ToTime,
                        AccessCount = th.AccessCount,
                        GuessCount = th.GuessCount
                    }).ToList()
            },
            TrafficStatisticsByDevices = new TrafficStatisticsByDeviceListViewModel
            {
                Items = devicesTrafficServiceResult.ResponseDto
                    .Select(d => new TrafficStatisticsByDeviceViewModel
                    {
                        DeviceName = d.DeviceName,
                        AccessCount = d.AccessCount
                    }).ToList()
            }
        };
        
        return View("Dashboard", model);
    }

    [HttpGet("/cai-dat-chung")]
    public async Task<IActionResult> GeneralSettings()
    {
        ServiceResult<GeneralSettingsResponseDto> serviceResult;
        serviceResult = await _generalSettingsService.GetAsync();
        GeneralSettingsViewModel model = new GeneralSettingsViewModel
        {
            ApplicationName = serviceResult.ResponseDto.ApplicationName,
            ApplicationShortName = serviceResult.ResponseDto.ApplicationShortName,
            FavIconUrl = serviceResult.ResponseDto.FavIconUrl,
            UnderMaintainance = serviceResult.ResponseDto.UnderMaintainance
        };
        return View("GeneralSettings", model);
    }

    [HttpPost("/cai-dat-chung")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GeneralSettings(GeneralSettingsViewModel model)
    {
        GeneralSettingsRequestDto requestDto = new GeneralSettingsRequestDto
        {
            ApplicationName = model.ApplicationName,
            ApplicationShortName = model.ApplicationShortName,
            FavIconFile = model.FavIconFile,
            UnderMaintainance = model.UnderMaintainance
        };
        ServiceResult<GeneralSettingsResponseDto> serviceResult;
        serviceResult = await _generalSettingsService.UpdateAsync(requestDto);
        if (!serviceResult.Succeeded)
        {
            ModelState.AddModelErrorsFromServiceErrors(serviceResult.Errors);
            return BadRequest(ModelState);
        }

        return Ok();
    }

    [HttpGet("noi-dung")]
    public async Task<IActionResult> Content()
    {
        ServiceResult<AboutUsIntroductionResponseDto> aboutUsIntroductionServiceResult;
        aboutUsIntroductionServiceResult = await _aboutUsIntroductionService.GetAsync();

        ServiceResult<List<TeamMemberResponseDto>> teamMemberServiceResult;
        teamMemberServiceResult = await _teamMembersService.GetListAsync();

        ServiceResult<List<BusinessCertificateResponseDto>> businessCertificateServiceResult;
        businessCertificateServiceResult = await _businessCertificateService.GetListAsync();

        ServiceResult<List<HomePageSliderItemResponseDto>> homePageSliderItemServiceResult;
        homePageSliderItemServiceResult = await _homePageSliderItemService.GetListAsync();

        ServiceResult<List<IntroductionItemResponseDto>> introductionItemServiceResult;
        introductionItemServiceResult = await _introductionItemService.GetListAsync();

        ServiceResult<ContactInfoResponseDto> contactInfoServiceResult;
        contactInfoServiceResult = await _contactInfoService.GetAsync();

        ContentViewModel model = new ContentViewModel
        {
            AboutUsIntroduction = new AboutUsIntroductionViewModel
            {
                MainPhotoUrl = aboutUsIntroductionServiceResult.ResponseDto.MainPhotoUrl,
                MainQuoteContent = aboutUsIntroductionServiceResult.ResponseDto.MainQuoteContent,
                AboutUsContent = aboutUsIntroductionServiceResult.ResponseDto.AboutUsContent,
                WhyChooseUsContent = aboutUsIntroductionServiceResult.ResponseDto.WhyChooseUsContent,
                OurDifferenceContent = aboutUsIntroductionServiceResult.ResponseDto.OurDifferenceContent,
                OurCultureContent = aboutUsIntroductionServiceResult.ResponseDto.OurCultureContent,
            },
            TeamMemberList = new TeamMemberListViewModel
            {
                TeamMembers = teamMemberServiceResult.ResponseDto
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
            },
            HomePageSliderItems = new HomePageSliderItemListViewModel
            {
                Items = homePageSliderItemServiceResult.ResponseDto
                    .Select(dto => new HomePageSliderItemViewModel
                    {
                        Id = dto.Id,
                        Title = dto.Title,
                        PhotoUrl = dto.PhotoUrl
                    }).ToList()
            },
            IntroductionItems = new IntroductionItemListViewModel
            {
                Items = introductionItemServiceResult.ResponseDto
                    .Select(ii => new IntroductionItemViewModel
                    {
                        Id = ii.Id,
                        Name = ii.Name,
                        Summary = ii.Summary,
                        Content = ii.Content,
                        ThumbnailUrl = ii.ThumbnailUrl
                    }).ToList()
            },
            ContactInfo = new ContactInfoViewModel
            {
                PhoneNumber = contactInfoServiceResult.ResponseDto.PhoneNumber,
                ZaloNumber = contactInfoServiceResult.ResponseDto.ZaloNumber,
                Email = contactInfoServiceResult.ResponseDto.Email,
                Address = contactInfoServiceResult.ResponseDto.Address
            }
        };
        return View("Content", model);
    }

    [HttpGet("noi-dung/ve-chung-toi")]
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

    [HttpPost("noi-dung/ve-chung-toi")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AboutUs(AboutUsIntroductionViewModel model)
    {
        // Map main photo file
        byte[] mainPhotoFile = null;
        if (model.MainPhotoFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await model.MainPhotoFile.CopyToAsync(stream);
            mainPhotoFile = stream.ToArray();
        }

        AboutUsIntroductionRequestDto requestDto;
        requestDto = new AboutUsIntroductionRequestDto
        {
            MainPhotoFile = mainPhotoFile,
            MainPhotoChanged = model.MainPhotoChanged,
            MainQuoteContent = model.MainQuoteContent,
            AboutUsContent = model.AboutUsContent,
            WhyChooseUsContent = model.WhyChooseUsContent,
            OurDifferenceContent = model.OurDifferenceContent,
            OurCultureContent = model.OurCultureContent
        };

        ServiceResult<AboutUsIntroductionResponseDto> serviceResult;
        serviceResult = await _aboutUsIntroductionService.UpdateAsync(requestDto);

        if (!serviceResult.Succeeded)
        {
            ModelState.AddModelErrorsFromServiceErrors(serviceResult.Errors);
            return BadRequest(ModelState);
        }

        return Ok();
    }

    [HttpGet("noi-dung/doi-ngu/tao-moi")]
    public IActionResult TeamMemberCreating()
    {
        TeamMemberViewModel model = new TeamMemberViewModel
        {
            IsForCreating = true
        };
        return View("TeamMember", model);
    }

    [HttpPost("noi-dung/doi-ngu/tao-moi")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TeamMemberCreating(TeamMemberViewModel model)
    {
        byte[] photoFile = null;
        if (model.PhotoFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await model.PhotoFile.CopyToAsync(stream);
            photoFile = stream.ToArray();
        }
        TeamMemberRequestDto requestDto = new TeamMemberRequestDto {
            PhotoFile = photoFile,
            FullName = model.FullName,
            RoleName = model.RoleName,
            Description = model.Description,
            PhotoChanged = model.PhotoChanged
        };

        ServiceResult<TeamMemberResponseDto> serviceResult;
        serviceResult = await _teamMembersService.CreateAsync(requestDto);
        if (!serviceResult.Succeeded)
        {
            ModelState.AddModelErrorsFromServiceErrors(serviceResult.Errors);
            return BadRequest(ModelState);
        }

        return Ok();
    }

    [HttpGet("noi-dung/doi-ngu/{id:int}/cap-nhat")]
    public async Task<IActionResult> TeamMemberUpdating(int id)
    {
        ServiceResult<TeamMemberResponseDto> serviceResult;
        serviceResult = await _teamMembersService.GetAsync(id);
        if (!serviceResult.Succeeded)
        {
            return NotFound();
        }
        
        TeamMemberViewModel model = new TeamMemberViewModel
        {
            Id = serviceResult.ResponseDto.Id,
            FullName = serviceResult.ResponseDto.FullName,
            RoleName = serviceResult.ResponseDto.RoleName,
            Description = serviceResult.ResponseDto.Description,
            PhotoUrl = serviceResult.ResponseDto.PhotoUrl,
            IsForCreating = false
        };
        return View("TeamMember", model);
    }

    [HttpPost("noi-dung/doi-ngu/{id:int}/cap-nhat")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TeamMemberUpdating(int id, TeamMemberViewModel model)
    {
        byte[] photoFile = null;
        if (model.PhotoFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await model.PhotoFile.CopyToAsync(stream);
            photoFile = stream.ToArray();
        }
        TeamMemberRequestDto requestDto = new TeamMemberRequestDto {
            PhotoFile = photoFile,
            FullName = model.FullName,
            RoleName = model.RoleName,
            Description = model.Description,
            PhotoChanged = model.PhotoChanged
        };

        ServiceResult<TeamMemberResponseDto> serviceResult;
        serviceResult = await _teamMembersService.UpdateAsync(id, requestDto);
        if (!serviceResult.Succeeded)
        {
            if (serviceResult.HasNotFoundError)
            {
                return NotFound();
            }
            ModelState.AddModelErrorsFromServiceErrors(serviceResult.Errors);
            return BadRequest(ModelState);
        }

        return Ok();
    }

    [HttpPost("noi-dung/doi-ngu/{id:int}/xoa-bo")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TeamMemberDeleting(int id)
    {
        ServiceResult<int> serviceResult = await _teamMembersService.DeleteAsync(id);
        if (!serviceResult.Succeeded)
        {
            return NotFound();
        }
        
        return Ok();
    }

    [HttpGet("noi-dung/chung-chi/tao-moi")]
    public IActionResult BusinessCertificateCreating()
    {
        BusinessCertificateViewModel model = new BusinessCertificateViewModel
        {
            IsForCreating = true
        };
        return View("BusinessCertificate", model);
    }

    [HttpPost("noi-dung/chung-chi/tao-moi")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BusinessCertificateCreating(BusinessCertificateViewModel model)
    {
        byte[] photoFile = null;
        if (model.PhotoFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await model.PhotoFile.CopyToAsync(stream);
            photoFile = stream.ToArray();
        }
        BusinessCertificateRequestDto requestDto = new BusinessCertificateRequestDto
        {
            PhotoFile = photoFile,
            Name = model.Name,
            PhotoChanged = model.PhotoChanged
        };
        ServiceResult<BusinessCertificateResponseDto> serviceResult;
        serviceResult = await _businessCertificateService.CreateAsync(requestDto);
        if (!serviceResult.Succeeded)
        {
            ModelState.AddModelErrorsFromServiceErrors(serviceResult.Errors);
            return BadRequest(ModelState);
        }

        return Ok();
    }

    [HttpGet("noi-dung/chung-chi/{id:int}/cap-nhat")]
    public async Task<IActionResult> BusinessCertificateUpdating(int id)
    {
        ServiceResult<BusinessCertificateResponseDto> serviceResult;
        serviceResult = await _businessCertificateService.GetAsync(id);
        if (!serviceResult.Succeeded)
        {
            return NotFound();
        }

        BusinessCertificateViewModel model = new BusinessCertificateViewModel
        {
            Id = serviceResult.ResponseDto.Id,
            Name = serviceResult.ResponseDto.Name,
            PhotoUrl = serviceResult.ResponseDto.PhotoUrl
        };
        
        return View("BusinessCertificate", model);
    }

    [HttpPost("noi-dung/chung-chi/{id:int}/cap-nhat")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BusinessCertificateUpdating(int id, BusinessCertificateViewModel model)
    {
        byte[] photoFile = null;
        if (model.PhotoFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await model.PhotoFile.CopyToAsync(stream);
            photoFile = stream.ToArray();
        }
        BusinessCertificateRequestDto requestDto = new BusinessCertificateRequestDto
        {
            PhotoFile = photoFile,
            Name = model.Name,
            PhotoChanged = model.PhotoChanged
        };
        ServiceResult<BusinessCertificateResponseDto> serviceResult;
        serviceResult = await _businessCertificateService.UpdateAsync(id, requestDto);
        if (!serviceResult.Succeeded)
        {
            if (serviceResult.HasNotFoundError)
            {
                return NotFound();
            }

            ModelState.AddModelErrorsFromServiceErrors(serviceResult.Errors);
            return BadRequest(ModelState);
        }

        return Ok();
    }

    [HttpPost("noi-dung/chung-chi/{id:int}/xoa-bo")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BusinessCertificateDeleting(int id)
    {
        ServiceResult<int> serviceResult = await _businessCertificateService.DeleteAsync(id);
        if (!serviceResult.Succeeded)
        {
            if (serviceResult.HasNotFoundError)
            {
                return NotFound(serviceResult.Errors);
            }
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet("gioi-thieu/{id:int}")]
    public async Task<IActionResult> IntroductionItemUpdating(int id)
    {
        ServiceResult<IntroductionItemResponseDto> serviceResult;
        serviceResult = await _introductionItemService.GetAsync(id);
        if (!serviceResult.Succeeded)
        {
            return NotFound();
        }

        IntroductionItemViewModel model = new IntroductionItemViewModel
        {
            Id = serviceResult.ResponseDto.Id,
            Name = serviceResult.ResponseDto.Name,
            Summary = serviceResult.ResponseDto.Summary,
            Content = serviceResult.ResponseDto.Content,
            ThumbnailUrl = serviceResult.ResponseDto.ThumbnailUrl
        };
        return View("~/Views/Admin/IntroductionItem.cshtml", model);
    }

    [HttpPost("gioi-thieu/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IntroductionItemUpdating(int id, IntroductionItemViewModel model)
    {
        byte[] thumbnailFile = null;
        if (model.ThumbnailFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await model.ThumbnailFile.CopyToAsync(stream);
            thumbnailFile = stream.ToArray();
        }
        IntroductionItemRequestDto requestDto = new IntroductionItemRequestDto
        {
            Name = model.Name,
            Summary = model.Summary,
            Content = model.Content,
            ThumbnailFile = thumbnailFile,
            ThumbnailChanged = model.ThumbnailChanged
        };
        ServiceResult<IntroductionItemResponseDto> serviceResult;
        serviceResult = await _introductionItemService.UpdateAsync(id, requestDto);

        if (!serviceResult.Succeeded)
        {
            if (serviceResult.HasNotFoundError)
            {
                return NotFound();
            }
            ModelState.AddModelErrorsFromServiceErrors(serviceResult.Errors);
            return BadRequest(ModelState);
        }

        return Ok();
    }

    [HttpGet("thong-tin-lien-he/cap-nhat")]
    public async Task<IActionResult> ContactInfoUpdating()
    {
        ServiceResult<ContactInfoResponseDto> serviceResult;
        serviceResult = await _contactInfoService.GetAsync();

        ContactInfoViewModel model = new ContactInfoViewModel
        {
            PhoneNumber = serviceResult.ResponseDto.PhoneNumber,
            ZaloNumber = serviceResult.ResponseDto.ZaloNumber,
            Email = serviceResult.ResponseDto.Email,
            Address = serviceResult.ResponseDto.Address
        };
        
        return View("~/Views/Admin/ContactInfo.cshtml", model);
    }

    [HttpPost("thong-tin-lien-he/cap-nhat")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ContactInfoUpdating(ContactInfoViewModel model)
    {
        ContactInfoRequestDto requestDto = new ContactInfoRequestDto
        {
            PhoneNumber = model.PhoneNumber,
            ZaloNumber = model.ZaloNumber,
            Email = model.Email,
            Address = model.Address
        };

        ServiceResult<ContactInfoResponseDto> serviceResult;
        serviceResult = await _contactInfoService.UpdateAsync(requestDto);
        if (!serviceResult.Succeeded)
        {
            ModelState.AddModelErrorsFromServiceErrors(serviceResult.Errors);
            return BadRequest(ModelState);
        }
        
        return Ok();
    }
}