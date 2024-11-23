namespace NATS.Models;

public class PostBasicListViewModel
{
    [Display(Name = DisplayNames.Statistics)]
    public PostListStatisticsViewModel Statistics { get; set; }
    
    [Display(Name = DisplayNames.Post)]
    public List<PostBasicViewModel> Items { get; set; }
    
    [Display(Name = DisplayNames.Page)]
    [BindNever]
    public int Page { get; set; }
    
    [Display(Name = DisplayNames.PageCount)]
    [BindNever]
    public int PageCount { get; set; }
}