namespace NATS.Models;

public class PostListStatisticsViewModel
{
    [Display(Name = DisplayNames.TotalPosts)]
    public int TotalPosts { get; set; }
    
    [Display(Name = DisplayNames.TotalViews)]
    public int TotalViews { get; set; }
    
    [Display(Name = DisplayNames.UnpublishedPosts)]
    public int UnpublishedPosts { get; set; }
}