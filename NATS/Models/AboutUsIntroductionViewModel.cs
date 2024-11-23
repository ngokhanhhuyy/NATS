namespace NATS.Models;

public class AboutUsIntroductionViewModel
{
    public string MainPhotoUrl { get; set; }
    public bool MainPhotoChanged { get; set; }

    [Required]
    public IFormFile MainPhotoFile { get; set; }

    [Display(Name = DisplayNames.MessageFromUs)]
    [Required]
    [MaxLength(1000)]
    public string MainQuoteContent { get; set; }

    [Display(Name = DisplayNames.AboutUs)]
    [Required]
    [MaxLength(1500)]
    public string AboutUsContent { get; set; }

    [Display(Name = DisplayNames.WhyChooseUs)]
    [Required]
    [MaxLength(1500)]
    public string WhyChooseUsContent { get; set; }
    
    [Display(Name = DisplayNames.OurDifference)]
    [Required]
    [MaxLength(1500)]
    public string OurDifferenceContent { get; set; }
    
    [Display(Name = DisplayNames.OurCulture)]
    [Required]
    [MaxLength(1500)]
    public string OurCultureContent { get; set; }
}