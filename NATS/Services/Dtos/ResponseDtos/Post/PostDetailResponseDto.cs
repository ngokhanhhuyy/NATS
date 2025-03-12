namespace NATS.Services.Dtos.ResponseDtos;

public class PostDetailResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string NormalizedTitle { get; set; }
    public string ThumbnailUrl { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime? UpdatedDateTime { get; set; }
    public bool IsPinned { get; set; }
    public bool IsPublished { get; set; }
    public int Views { get; set; }
    public UserBasicResponseDto User { get; set; }

    public PostDetailResponseDto(Post post)
    {
        Id = post.Id;
        Title = post.Title;
        NormalizedTitle = post.NormalizedTitle;
        ThumbnailUrl = post.ThumbnailUrl;
        Content = post.Content;
        CreatedDateTime = post.CreatedDateTime;
        UpdatedDateTime = post.UpdatedDateTime;
        IsPinned = post.IsPinned;
        IsPublished = post.IsPublished;
        Views = post.Views;
        User = new UserBasicResponseDto(post.User);
    }
}