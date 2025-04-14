namespace NATS.Services.Dtos.RequestDtos;

public class CatalogItemUpsertRequestDto : IHasThumbnailUpsertRequestDto
{
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Detail { get; set; }
    public byte[] ThumbnailFile { get; set; }
    public bool ThumbnailChanged{ get; set; }
    public List<CatalogItemUpsertPhotoRequestDto> Photos { get; set; }

    public void TransformValues()
    {
        Name = Name.ToNullIfEmpty();
        Summary = Summary.ToNullIfEmpty();
        Detail = Detail.ToNullIfEmpty();

        if (Photos != null)
        {
            foreach (CatalogItemUpsertPhotoRequestDto photo in Photos)
            {
                photo.TransformValues();
            }
        }
    }
}