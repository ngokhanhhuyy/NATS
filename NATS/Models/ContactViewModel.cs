namespace NATS.Models;

public class ContactViewModel
{
    [Display(Name = DisplayNames.ContactInfo)]
    public ContactInfoViewModel ContactInfo { get; set; }
    
    [Display(Name = DisplayNames.Enquiry)]
    public EnquiryViewModel Enquiry { get; set; }
}