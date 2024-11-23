namespace NATS.Models;

public class HomePageSliderItemViewModel
{
    [Display(Name = DisplayNames.Id)]
    public int? Id { get; set; }

    [Display(Name = DisplayNames.Title)]
    [MaxLength(100)]
    public string Title { get; set; }

    [Display(Name = DisplayNames.Photo)]
    public string PhotoUrl { get; set; }

    [Display(Name = DisplayNames.Photo)]
    public IFormFile PhotoFile { get; set; }

    public bool PhotoChanged { get; set; } = false;

    [BindNever]
    public bool IsForCreating { get; set; }
}