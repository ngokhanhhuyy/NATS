namespace NATS.Services.Dtos.RequestDtos;

public class AboutUsIntroductionUpsertRequestDto
        : IHasThumbnailUpsertRequestDto<AboutUsIntroductionUpsertRequestDto>
{
    public byte[] ThumbnailFile { get; set; }
    public bool ThumbnailChanged { get; set; }
    public string MainQuoteContent { get; set; }
    public string AboutUsContent { get; set; }
    public string WhyChooseUsContent { get; set; }
    public string OurDifferenceContent { get; set; }
    public string OurCultureContent { get; set; }

    public AboutUsIntroductionUpsertRequestDto TransformValues()
    {
        MainQuoteContent = MainQuoteContent.ToNullIfEmpty();
        AboutUsContent = AboutUsContent.ToNullIfEmpty();
        WhyChooseUsContent = WhyChooseUsContent.ToNullIfEmpty();
        OurDifferenceContent = OurDifferenceContent.ToNullIfEmpty();
        OurCultureContent = OurCultureContent.ToNullIfEmpty();
        return this;
    }
}