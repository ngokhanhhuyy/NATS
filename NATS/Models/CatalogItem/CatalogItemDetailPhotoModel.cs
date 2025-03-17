namespace NATS.Models;

public class CatalogItemDetailPhotoModel
{
    public int Id { get; set; }
    public string Url { get; set; }

    public CatalogItemDetailPhotoModel(CatalogItemDetailPhotoResponseDto responseDto)
    {
        Id = responseDto.Id;
        Url = responseDto.Url;
    }
}