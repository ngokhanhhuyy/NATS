namespace NATS.Public.Models;

public class SummaryItemListViewModel
{
    public List<SummaryItemDetailModel> Items { get; set; }
    public int? FocusedId { get; set; }
    
    public SummaryItemListViewModel(
            List<SummaryItemResponseDto> responseDtos,
            int? focusedId = null)
    {
        Items = responseDtos.Select(dto => new SummaryItemDetailModel(dto)).ToList();
        FocusedId = focusedId;
    }
}