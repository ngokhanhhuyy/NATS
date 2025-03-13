namespace NATS.Services.Dtos.RequestDtos;

public class SummaryItemUpsertRequestDto : IHasThumbnailUpsertRequestDto
{
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Content { get; set; }
    public byte[] ThumbnailFile { get; set; }
    public bool ThumbnailChanged { get; set; }

    public void TransformValues()
    {
        Name = Name.ToNullIfEmpty();
        Summary = Summary.ToNullIfEmpty();
        Content = Content.ToNullIfEmpty();
    }
}
