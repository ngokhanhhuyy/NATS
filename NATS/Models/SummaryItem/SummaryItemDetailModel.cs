namespace NATS.Models;

public class SummaryItemDetailModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SummaryContent { get; set; }
    public string DetailContent { get; set; }
    public string ThumbnailUrl { get; set; }

    public SummaryItemDetailModel(SummaryItemResponseDto responseDto)
    {
        Id = responseDto.Id;
        Name = responseDto.Name;
        SummaryContent = responseDto.SummaryContent;
        DetailContent = responseDto.DetailContent;
        ThumbnailUrl = responseDto.ThumbnailUrl;
    }

    public string GetPublicDetailRouteName(IUrlHelper urlHelper)
    {
        return urlHelper.GetPublicSummaryItemListRoutePath(Id);
    }
}