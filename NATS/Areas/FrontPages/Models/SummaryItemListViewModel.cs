namespace NATS.FrontPages.Models;

public class SummaryItemListViewModel
{
    public List<SummaryItemDetailModel> Items { get; set; }
    
    public SummaryItemListViewModel(List<SummaryItemResponseDto> responseDtos)
    {
        Items = responseDtos.Select(dto => new SummaryItemDetailModel(dto)).ToList();
    }
}