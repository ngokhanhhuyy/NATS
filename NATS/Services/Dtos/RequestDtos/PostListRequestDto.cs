namespace NATS.Services.Dtos.RequestDtos;

public class PostListRequestDto : IRequestDto
{
    public bool OrderByAscending { get; set; } = true;
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    
    public void TransformValues()
    {
    }
}