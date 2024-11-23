namespace NATS.Services.Dtos.ResponseDtos;

public class PostBasicResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string NormalizedTitle { get; set; }
    public string ThumbnailUrl { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public bool IsPinned { get; set; }
    public bool IsPublished { get; set; }
    public int Views { get; set; }
}