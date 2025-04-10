namespace NATS.Services.Dtos.ResponseDtos;

public class CatalogItemDetailPhotoResponseDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }

    public CatalogItemDetailPhotoResponseDto(CatalogItemPhoto photo)
    {
        Id = photo.Id;
        Description = photo.Description;
        Url = photo.Url;
    }
}