namespace NATS.Models;

public class FooterViewModel
{
    [Display(Name = DisplayNames.GeneralSettings)]
    public GeneralSettingsViewModel GeneralSettings { get; set; }

    [Display(Name = DisplayNames.Post)]
    public PostBasicListViewModel Posts { get; set; }

    [Display(Name = DisplayNames.ContactInfo)]
    public ContactInfoViewModel ContactInfo { get; set; }
}