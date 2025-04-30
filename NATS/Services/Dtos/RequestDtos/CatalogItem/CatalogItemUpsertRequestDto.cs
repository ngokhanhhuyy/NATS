namespace NATS.Services.Dtos.RequestDtos;

public class CatalogItemUpsertRequestDto : IHasThumbnailUpsertRequestDto
{
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Detail { get; set; }
    public string ThumbnailUrl { get; set; }

    public void TransformValues()
    {
        Name = Name.ToNullIfEmpty();
        Summary = Summary.ToNullIfEmpty();
        Detail = Detail.ToNullIfEmpty();
        ThumbnailUrl = ThumbnailUrl.ToNullIfEmpty();
    }
}