namespace NATS.Models;

public class AboutUsIntroductionUpsertModel : IHasThumbnailUpsertModel
{
    public string ThumbnailUrl { get; set; }
    
    public bool ThumbnailChanged { get; set; }

    [Required]
    public IFormFile ThumbnailFile { get; set; }

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

    public AboutUsIntroductionUpsertModel(AboutUsIntroductionResponseDto responseDto)
    {
        ThumbnailUrl = responseDto.ThumbnailUrl;
        MainQuoteContent = responseDto.MainQuoteContent;
        AboutUsContent = responseDto.AboutUsContent;
        WhyChooseUsContent = responseDto.WhyChooseUsContent;
    }
}