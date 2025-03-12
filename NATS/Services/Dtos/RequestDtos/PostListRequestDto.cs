namespace NATS.Services.Dtos.RequestDtos;

public class PostListRequestDto : IRequestDto<PostListRequestDto>
{
    public bool OrderByAscending { get; set; } = true;
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    
    public PostListRequestDto TransformValues()
    {
        return this;
    }
}