namespace NATS.Models;

public class PostBasicViewModel
{
    [Display(Name = DisplayNames.Id)]
    public int Id { get; set; }
    
    [Display(Name = DisplayNames.Title)]
    public string Title { get; set; }
    
    [Display(Name = DisplayNames.NormalizedTitle)]
    public string NormalizedTitle { get; set; }
    
    [Display(Name = DisplayNames.Thumbnail)]
    public string ThumbnailUrl { get; set; }
    
    [Display(Name = DisplayNames.Content)]
    public string Content { get; set; }
    
    [Display(Name = DisplayNames.CreatedDateTime)]
    public DateTime CreatedDateTime { get; set; }
    
    [Display(Name = DisplayNames.IsPublished)]
    public bool IsPublished { get; set; }
    
    [Display(Name = DisplayNames.IsPinned)]
    public bool IsPinned { get; set; }
    
    [Display(Name = DisplayNames.Views)]
    public int Views { get; set; }
}