namespace NATS.Models;

public class ContactModel
{
    [Display(Name = DisplayNames.ContactInfo)]
    public ContactInfoViewModel ContactInfo { get; set; }
    
    [Display(Name = DisplayNames.Enquiry)]
    public EnquiryModel Enquiry { get; set; }

    public ContactModel(ContactResponseDto responseDto)
    {
        Conta
    }
}