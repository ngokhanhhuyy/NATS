using NATS.FrontPages.Models;

namespace NATS.Areas.FrontPages.Controllers;

[Area("FrontPages")]
[Route("/gioi-thieu")]
public class AboutUsController : Controller
{
    private readonly IAboutUsIntroductionService _aboutUsIntroductionService;
    private readonly IMemberService _memberService;

    public AboutUsController(
            IAboutUsIntroductionService aboutusIntroductionService,
            IMemberService memberService)
    {
        _aboutUsIntroductionService = aboutusIntroductionService;
        _memberService = memberService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        Task<AboutUsIntroductionResponseDto> aboutUsIntroductionResponseDtoTask;
        aboutUsIntroductionResponseDtoTask = _aboutUsIntroductionService.GetAsync();

        Task<List<MemberResponseDto>> memberResponseDtosTask;
        memberResponseDtosTask = _memberService.GetListAsync();

        await Task.WhenAll(aboutUsIntroductionResponseDtoTask, memberResponseDtosTask);

        AboutUsViewModel model = new AboutUsViewModel(
            aboutUsIntroductionResponseDtoTask.Result,
            memberResponseDtosTask.Result);

        return View(model);
    }
}