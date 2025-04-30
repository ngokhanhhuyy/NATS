namespace NATS.Services.Dtos.ResponseDtos;

public class CatalogItemDetailResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CatalogItemType Type { get; set; }
    public string Summary { get; set; }
    public string Detail { get; set; }
    public string ThumbnailUrl { get; set; }
    public List<CatalogItemBasicResponseDto> OtherItems { get; set; }

    public CatalogItemDetailResponseDto(CatalogItem item, List<CatalogItem> otherItems)
    {
        Id = item.Id;
        Name = item.Name;
        Type = item.Type;
        Summary = item.Summary;
        Detail = item.Detail;
        ThumbnailUrl = item.ThumbnailUrl;
        OtherItems = otherItems
            .Select(dto => new CatalogItemBasicResponseDto(dto))
            .ToList();
    }
}