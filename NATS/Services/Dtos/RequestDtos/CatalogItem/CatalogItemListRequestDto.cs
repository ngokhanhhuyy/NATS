namespace NATS.Services.Dtos.RequestDtos;

public class CatalogItemListRequestDto : IRequestDto
{
    public CatalogItemType? Type { get; set; }
    public List<int> ExcludedIds { get; set; }
}