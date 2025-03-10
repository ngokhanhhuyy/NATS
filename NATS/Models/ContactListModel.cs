namespace NATS.Models;

public class ContactListModel
{
    [Display(Name = DisplayNames.ContactInfo)]
    public ContactModel ContactInfo { get; set; }
    
    [Display(Name = DisplayNames.Enquiry)]
    public EnquiryModel Enquiry { get; set; }
}