namespace NATS.FrontPages.Models;

public class CatalogItemListViewModel
{
    public List<CatalogItemBasicModel> Items { get; set; }
    public CatalogItemType Type { get; set; }

    public CatalogItemListViewModel(
            List<CatalogItemBasicResponseDto> responseDtos,
            CatalogItemType type)
    {
        Items = responseDtos.Select(dto => new CatalogItemBasicModel(dto)).ToList();
        Type = type;
    }
}