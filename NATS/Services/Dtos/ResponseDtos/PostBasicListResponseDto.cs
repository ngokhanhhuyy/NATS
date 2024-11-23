namespace NATS.Services.Dtos.ResponseDtos;

public class PostBasicListResponseDto
{
    public List<PostBasicResponseDto> Items { get; set; }
    public int PageCount { get; set; }
}