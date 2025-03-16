namespace NATS.FrontPages.Models;

public class AboutUsViewModel
{
    public AboutUsIntroductionDetailModel AboutUsIntroduction { get; set; }
    public List<MemberDetailModel> Members { get; set; }

    public AboutUsViewModel() { }

    public AboutUsViewModel(
            AboutUsIntroductionResponseDto aboutUsIntroductionResponseDto,
            List<MemberResponseDto> memberResponseDtos)
    {
        AboutUsIntroduction = new AboutUsIntroductionDetailModel(
            aboutUsIntroductionResponseDto);
        Members = memberResponseDtos
            .Select(dto => new MemberDetailModel(dto))
            .ToList();
    }
}