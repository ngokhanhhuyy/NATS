namespace NATS.Models;

public class AboutUsIntroductionDetailModel
{
    [Display(Name = DisplayNames.Thumbnail)]
    public string ThumbnailUrl { get; set; }

    [Display(Name = DisplayNames.MessageFromUs)]
    public string MainQuoteContent { get; set; }

    [Display(Name = DisplayNames.AboutUs)]
    public string AboutUsContent { get; set; }

    [Display(Name = DisplayNames.WhyChooseUs)]
    public string WhyChooseUsContent { get; set; }
    
    [Display(Name = DisplayNames.OurDifference)]
    public string OurDifferenceContent { get; set; }
    
    [Display(Name = DisplayNames.OurCulture)]
    public string OurCultureContent { get; set; }

    public AboutUsIntroductionDetailModel(AboutUsIntroductionResponseDto responseDto)
    {
        ThumbnailUrl = responseDto.ThumbnailUrl;
        MainQuoteContent = responseDto.MainQuoteContent;
        AboutUsContent = responseDto.AboutUsContent;
        WhyChooseUsContent = responseDto.WhyChooseUsContent;
        OurDifferenceContent = responseDto.OurDifferenceContent;
        OurCultureContent = responseDto.OurCultureContent;
    }
}