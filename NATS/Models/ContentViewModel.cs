namespace NATS.Models;

public class ContentViewModel
{
    [Display(Name = DisplayNames.AboutUs)]
    public AboutUsIntroductionViewModel AboutUsIntroduction { get; set; }

    [Display(Name = DisplayNames.TeamMembers)]
    public TeamMemberListViewModel TeamMemberList { get; set; }

    [Display(Name = DisplayNames.HomePageSliderItem)]
    public HomePageSliderItemListViewModel HomePageSliderItems { get; set; }

    [Display(Name = DisplayNames.IntroductionItem)]
    public IntroductionItemListViewModel IntroductionItems { get; set; }

    [Display(Name = DisplayNames.ContactInfo)]
    public ContactInfoViewModel ContactInfo { get; set; }
}