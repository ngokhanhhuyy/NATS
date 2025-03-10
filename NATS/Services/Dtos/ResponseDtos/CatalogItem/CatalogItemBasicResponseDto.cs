namespace NATS.Services.Dtos.ResponseDtos;

public class CatalogItemBasicResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CatalogItemType Type { get; set; }
    public string Summary { get; set; }
    public string ThumbnailUrl { get; set; }

    public CatalogItemBasicResponseDto(CatalogItem item)
    {
        Id = item.Id;
        Name = item.Name;
        Type = item.Type;
        Summary = item.Summary;
        ThumbnailUrl = item.ThumbnailUrl;
    }
}