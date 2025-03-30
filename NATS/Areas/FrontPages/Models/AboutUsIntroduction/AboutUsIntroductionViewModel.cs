namespace NATS.FrontPages.Models;

public class AboutUsIntroductionViewModel
{
    public AboutUsIntroductionDetailModel AboutUsIntroduction { get; set; }
    public List<MemberDetailModel> Members { get; set; }
    public List<CertificateDetailModel> Certificates { get; set; }

    public AboutUsIntroductionViewModel() { }

    public AboutUsIntroductionViewModel(
            AboutUsIntroductionResponseDto aboutUsIntroductionResponseDto,
            List<MemberResponseDto> memberResponseDtos,
            List<CertificateResponseDto> certificateResponseDtos)
    {
        AboutUsIntroduction = new AboutUsIntroductionDetailModel(
            aboutUsIntroductionResponseDto);
        Members = memberResponseDtos
            .Select(dto => new MemberDetailModel(dto))
            .ToList();
        Certificates = certificateResponseDtos
            .Select(dto => new CertificateDetailModel(dto))
            .ToList();
    }
}