namespace NATS.Services.Dtos.RequestDtos;

public class SummaryItemUpdateRequestDto : IHasThumbnailUpsertRequestDto
{
    public string Name { get; set; }
    public string SummaryContent { get; set; }
    public string DetailContent { get; set; }
    public string ThumbnailUrl { get; set; }

    public void TransformValues()
    {
        Name = Name.ToNullIfEmpty();
        SummaryContent = SummaryContent.ToNullIfEmpty();
        DetailContent = DetailContent.ToNullIfEmpty();
        ThumbnailUrl = ThumbnailUrl.ToNullIfEmpty();
    }
}
