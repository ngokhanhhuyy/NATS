namespace NATS.Models;

public class EnquiryViewModel
{
    [Display(Name = DisplayNames.Id)]
    [BindNever]
    public int Id { get; set; }
    
    [Display(Name = DisplayNames.FullName)]
    [Required]
    [MaxLength(50)]
    public string FullName { get; set; }
    
    [Display(Name = DisplayNames.Email)]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; }
    
    [Display(Name = DisplayNames.PhoneNumber)]
    [EmailAddress]
    [Required]
    [MaxLength(15)]
    public string PhoneNumber { get; set; }
    
    [Display(Name = DisplayNames.Content)]
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; }
    
    [Display(Name = DisplayNames.ReceivedDateTime)]
    [DataType(DataType.DateTime)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
    [BindNever]
    public DateTime ReceivedDateTime { get; set; }
    
    [Display(Name = DisplayNames.IsCompleted)]
    [BindNever]
    public bool IsCompleted { get; set; }
}