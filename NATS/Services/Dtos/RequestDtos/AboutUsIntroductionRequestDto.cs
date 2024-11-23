namespace NATS.Services.Dtos.RequestDtos;

public class AboutUsIntroductionRequestDto : IRequestDto<AboutUsIntroductionRequestDto>
{
    public byte[] MainPhotoFile { get; set; }
    public bool MainPhotoChanged { get; set; }
    public string MainQuoteContent { get; set; }
    public string AboutUsContent { get; set; }
    public string WhyChooseUsContent { get; set; }
    public string OurDifferenceContent { get; set; }
    public string OurCultureContent { get; set; }

    public AboutUsIntroductionRequestDto TransformValues()
    {
        MainQuoteContent = MainQuoteContent.ToNullIfEmpty();
        AboutUsContent = AboutUsContent.ToNullIfEmpty();
        WhyChooseUsContent = WhyChooseUsContent.ToNullIfEmpty();
        OurDifferenceContent = OurDifferenceContent.ToNullIfEmpty();
        OurCultureContent = OurCultureContent.ToNullIfEmpty();
        return this;
    }
}