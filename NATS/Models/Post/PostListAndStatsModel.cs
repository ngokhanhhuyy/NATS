namespace NATS.Models;

public class PostListAndStatsModel
{
    [Display(Name = DisplayNames.Statistics)]
    [BindNever]
    public PostStatsModel Stats { get; set; }

    [Display(Name = DisplayNames.Post)]
    public PostListModel Items { get; set; }

    public PostListAndStatsModel() { }

    public PostListAndStatsModel(
            PostStatsResponseDto statsResponseDto,
            PostListResponseDto listResponseDto = null)
    {
        Stats = new PostStatsModel(statsResponseDto);
        Items = new PostListModel();

        if (listResponseDto != null)
        {
            Items.MapFromResponseDto(listResponseDto);
        }
    }
}