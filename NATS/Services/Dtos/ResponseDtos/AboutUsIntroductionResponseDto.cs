namespace NATS.Services.Dtos.ResponseDtos;

public class AboutUsIntroductionResponseDto
{
    public string ThumbnailUrl { get; set; }
    public ThumbnailType ThumbnailType { get; set; }
    public string MainQuoteContent { get; set; }
    public string AboutUsContent { get; set; }
    public string WhyChooseUsContent { get; set; }
    public string OurDifferenceContent { get; set; }
    public string OurCultureContent { get; set; }

    public AboutUsIntroductionResponseDto(AboutUsIntroduction introduction)
    {
        ThumbnailUrl = introduction.ThumbnailUrl;
        ThumbnailType = introduction.ThumbnailType;
        MainQuoteContent = introduction.MainQuoteContent;
        AboutUsContent = introduction.AboutUsContent;
        WhyChooseUsContent = introduction.WhyChooseUsContent;
        OurDifferenceContent = introduction.OurDifferenceContent;
        OurCultureContent = introduction.OurCultureContent;
    }
}