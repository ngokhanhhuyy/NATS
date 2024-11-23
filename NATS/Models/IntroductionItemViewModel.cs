namespace NATS.Models;

public class IntroductionItemViewModel
{
    [Display(Name = DisplayNames.Id)]
    public int Id { get; set; }

    [Display(Name = DisplayNames.Name)]
    [Required]
    [MaxLength(25)]
    public string Name { get; set; }

    [Display(Name = DisplayNames.Summary)]
    [Required]
    [MaxLength(255)]
    public string Summary { get; set; }

    [Display(Name = DisplayNames.Content)]
    [Required]
    [MaxLength(3000)]
    public string Content { get; set; }

    [Display(Name = DisplayNames.Thumbnail)]
    public string ThumbnailUrl { get; set; }

    [Display(Name = DisplayNames.Photo)]
    public IFormFile ThumbnailFile { get; set; }

    public bool ThumbnailChanged { get; set; } = false;
}