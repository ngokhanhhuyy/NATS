namespace NATS.Models;

public class MembersAndCertificatesModel
{
    [Display(Name = DisplayNames.Members)]
    public List<MemberDetailModel> Members { get; set; }

    [Display(Name = DisplayNames.Certificates)]
    public List<CertificateDetailModel> Certificates { get; set; }

    public MembersAndCertificatesModel(
            List<MemberResponseDto> memberResponseDtos,
            List<CertificateResponseDto> certificateResponseDtos)
    {
        Members = memberResponseDtos
            .Select(dto => new MemberDetailModel(dto))
            .ToList();
        Certificates = certificateResponseDtos
            .Select(dto => new CertificateDetailModel(dto))
            .ToList();
    }
}