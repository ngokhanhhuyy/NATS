namespace NATS.Services.Dtos.RequestDtos;

public class CatalogItemUpsertRequestDto : IRequestDto<CatalogItemUpsertRequestDto>
{
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Detail { get; set; }
    public string ThumbnailUrl { get; set; }
    public byte[] ThumbnailFile { get; set; }
    public bool ThumbnailChanged{ get; set; }
    public List<CatalogItemUpsertPhotoRequestDto> Photos { get; set; }

    public CatalogItemUpsertRequestDto TransformValues()
    {
        Name = Name.ToNullIfEmpty();
        Summary = Summary.ToNullIfEmpty();
        Detail = Detail.ToNullIfEmpty();
        Photos = Photos?.Select(photo => photo.TransformValues()).ToList();
        return this;
    }
}