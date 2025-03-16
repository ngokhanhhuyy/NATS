namespace NATS.Models;

public class CatalogItemBasicModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CatalogItemType Type { get; set; }
    public string Summary { get; set; }
    public string ThumbnailUrl { get; set; }

    public CatalogItemBasicModel(CatalogItemBasicResponseDto responseDto)
    {
        Id = responseDto.Id;
        Name = responseDto.Name;
        Type = responseDto.Type;
        Summary = responseDto.Summary;
        ThumbnailUrl = responseDto.ThumbnailUrl;
    }
}