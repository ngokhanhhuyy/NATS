namespace NATS.Services.Dtos.ResponseDtos;

public class PostListResponseDto
{
    public List<PostBasicResponseDto> Results { get; set; } = new List<PostBasicResponseDto>();
    public int PageCount { get; set; }
}