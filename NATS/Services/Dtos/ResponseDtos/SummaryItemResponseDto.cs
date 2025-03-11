namespace NATS.Services.Dtos.ResponseDtos;

public class SummaryItemResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SummaryContent { get; set; }
    public string DetailContent { get; set; }
    public string ThumbnailUrl { get; set; }

    public SummaryItemResponseDto(SummaryItem summaryItem)
    {
        Id = summaryItem.Id;
        Name = summaryItem.Name;
        SummaryContent = summaryItem.SummaryContent;
        DetailContent = summaryItem.DetailContent;
        ThumbnailUrl = summaryItem.ThumbnailUrl;
    }
}