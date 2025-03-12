namespace NATS.Models;

public class PostDetailModel
{
    [Display(Name = DisplayNames.Id)]
    public int Id { get; set; }
    
    [Display(Name = DisplayNames.Title)]
    public string Title { get; set; }
    
    [Display(Name = DisplayNames.Thumbnail)]
    public string ThumbnailUrl { get; set; }
    
    [Display(Name = DisplayNames.Content)]
    public string Content { get; set; }
    
    [Display(Name = DisplayNames.CreatedDateTime)]
    public DateTime CreatedDateTime { get; set; }
    
    [Display(Name = DisplayNames.UpdatedDateTime)]
    public DateTime? UpdatedDateTime { get; set; }
    
    [Display(Name = DisplayNames.IsPinned)]
    public bool IsPinned { get; set; }
    
    [Display(Name = DisplayNames.IsPublished)]
    public bool IsPublished { get; set; }
    
    [Display(Name = DisplayNames.Views)]
    public int Views { get; set; }
    
    [Display(Name = DisplayNames.User)]
    public UserDetailModel User { get; set; }

    public PostDetailModel(PostDetailResponseDto post)
    {
        Id = post.Id;
        Title = post.Title;
        ThumbnailUrl = post.ThumbnailUrl;
        Content = post.Content;
        CreatedDateTime = post.CreatedDateTime;
        UpdatedDateTime = post.UpdatedDateTime;
        IsPinned = post.IsPinned;
        IsPublished = post.IsPublished;
        Views = post.Views;
        User = new UserDetailModel(post.User);
    }
}