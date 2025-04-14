namespace NATS.Protected.Models;

public class ContentViewModel
{
    [Display(Name = DisplayNames.SliderItem)]
    public List<SliderItemDetailModel> SliderItems { get; set; }
    
    [Display(Name = DisplayNames.SummaryItem)]
    public List<SummaryItemDetailModel> SummaryItems { get; set; }

    [Display(Name = DisplayNames.AboutUs)]
    public AboutUsIntroductionDetailModel AboutUsIntroduction { get; set; }

    [Display(Name = DisplayNames.Members)]
    public List<MemberDetailModel> Members { get; set; }

    [Display(Name = DisplayNames.Certificates)]
    public List<CertificateDetailModel> Certificates { get; set; }

    [Display(Name = DisplayNames.GeneralSettings)]
    public GeneralSettingsDetailModel GeneralSettings { get; set; }
}