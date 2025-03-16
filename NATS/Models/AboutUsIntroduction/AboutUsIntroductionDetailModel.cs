namespace NATS.Models;

public class AboutUsIntroductionDetailModel
{
    public string ThumbnailUrl { get; set; }
    public ThumbnailType ThumbnailType { get; set; }
    public string MainQuoteContent { get; set; }
    public string AboutUsContent { get; set; }
    public string WhyChooseUsContent { get; set; }
    public string OurDifferenceContent { get; set; }
    public string OurCultureContent { get; set; }

    public AboutUsIntroductionDetailModel(AboutUsIntroductionResponseDto responseDto)
    {
        ThumbnailUrl = responseDto.ThumbnailUrl;
        ThumbnailType = responseDto.ThumbnailType;
        MainQuoteContent = responseDto.MainQuoteContent;
        AboutUsContent = responseDto.AboutUsContent;
        WhyChooseUsContent = responseDto.WhyChooseUsContent;
        OurDifferenceContent = responseDto.OurDifferenceContent;
        OurCultureContent = responseDto.OurCultureContent;
    }
}