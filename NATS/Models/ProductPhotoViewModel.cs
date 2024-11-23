namespace NATS.Models;

public class ProductPhotoViewModel
{
    [Display(Name = DisplayNames.Id)]
    public int? Id { get; set; }

    [Display(Name = DisplayNames.Photo)]
    public string Url { get; set; }

    [Display(Name = DisplayNames.PhotoFile)]
    public IFormFile File { get; set; }

    public bool IsDeleted { get; set; } = false;
}
