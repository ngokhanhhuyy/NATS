namespace NATS.Models;

public class BusinessCertificateViewModel
{
    [Display(Name = DisplayNames.Id)]
    public int Id { get; set; }
    
    [Display(Name = DisplayNames.Name)]
    [MaxLength(100)]
    public string Name { get; set; }

    [Display(Name = DisplayNames.Photo)]
    public string PhotoUrl { get; set; }

    [Display(Name = DisplayNames.Photo)]
    public IFormFile PhotoFile { get; set; }

    public bool PhotoChanged { get; set; } = false;

    [BindNever]
    public bool IsForCreating { get; set; }
}