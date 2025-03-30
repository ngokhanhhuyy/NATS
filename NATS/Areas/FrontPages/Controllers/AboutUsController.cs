using NATS.FrontPages.Models;

namespace NATS.FrontPages.Controllers;

[Area("FrontPages")]
[Route("/ve-chung-toi")]
public class AboutUsController : Controller
{
    private readonly IAboutUsIntroductionService _aboutUsIntroductionService;
    private readonly IMemberService _memberService;
    private readonly ICertificateService _cerificateService;

    public AboutUsController(
            IAboutUsIntroductionService aboutusIntroductionService,
            IMemberService memberService,
            ICertificateService certificateService)
    {
        _aboutUsIntroductionService = aboutusIntroductionService;
        _memberService = memberService;
        _cerificateService = certificateService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        Task<AboutUsIntroductionResponseDto> aboutUsIntroductionResponseDtoTask;
        aboutUsIntroductionResponseDtoTask = _aboutUsIntroductionService.GetAsync();

        Task<List<MemberResponseDto>> memberResponseDtosTask;
        memberResponseDtosTask = _memberService.GetListAsync();

        Task<List<CertificateResponseDto>> certificateResponseDtosTask;
        certificateResponseDtosTask = _cerificateService.GetListAsync();

        await Task.WhenAll(
            aboutUsIntroductionResponseDtoTask,
            memberResponseDtosTask,
            certificateResponseDtosTask);

        AboutUsIntroductionViewModel model = new AboutUsIntroductionViewModel(
            aboutUsIntroductionResponseDtoTask.Result,
            memberResponseDtosTask.Result,
            certificateResponseDtosTask.Result);

        return View(model);
    }
}