namespace NATS.Services.Dtos.RequestDtos;

public class SummaryItemUpsertRequestDto : IHasThumbnailUpsertRequestDto
{
    public string Name { get; set; }
    public string SummaryContent { get; set; }
    public string DetailContent { get; set; }
    public byte[] ThumbnailFile { get; set; }
    public bool ThumbnailChanged { get; set; }

    public void TransformValues()
    {
        Name = Name.ToNullIfEmpty();
        SummaryContent = SummaryContent.ToNullIfEmpty();
        DetailContent = DetailContent.ToNullIfEmpty();
    }
}
