namespace NATS.Models;

public class SliderItemDetailModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Index { get; set; }
    public string ThumbnailUrl { get; set; }

    public SliderItemDetailModel(SliderItemResponseDto responseDto)
    {
        Id = responseDto.Id;
        Title = responseDto.Title;
        Index = responseDto.Index;
        ThumbnailUrl = responseDto.ThumbnailUrl;
    }

    public string GetUpdateRoutePath(IUrlHelper urlHelper)
    {
        string routeName = Protected.Controllers.SliderItemController.UpdateRouteName;
        return urlHelper.RouteUrl(routeName, new { id = Id });
    }
}