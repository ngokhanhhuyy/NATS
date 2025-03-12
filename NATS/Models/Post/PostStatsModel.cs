namespace NATS.Models;

public class PostStatsModel
{
    [Display(Name = DisplayNames.TotalPostCount)]
    public int TotalCount { get; set; }
    
    [Display(Name = DisplayNames.TotalPostViews)]
    public int TotalViews { get; set; }
    
    [Display(Name = DisplayNames.UnpublishedPostCount)]
    public int UnpublishedCount { get; set; }

    public PostStatsModel(PostStatsResponseDto responseDto)
    {
        TotalCount = responseDto.TotalCount;
        TotalViews = responseDto.TotalViews;
        UnpublishedCount = responseDto.UnpublishedCount;
    }
}