namespace NATS.Models;

public class ContactInfoViewModel
{
    [Display(Name = DisplayNames.PhoneNumber)]
    [DataType(DataType.PhoneNumber)]
    [MaxLength(20)]
    public string PhoneNumber { get; set; }
    
    [Display(Name = DisplayNames.Zalo)]
    [DataType(DataType.PhoneNumber)]
    [MaxLength(20)]
    public string ZaloNumber { get; set; }
    
    [Display(Name = DisplayNames.Email)]
    [DataType(DataType.EmailAddress)]
    [MaxLength(255)]
    public string Email { get; set; }
    
    [Display(Name = DisplayNames.Address)]
    [MaxLength(255)]
    public string Address { get; set; }
}