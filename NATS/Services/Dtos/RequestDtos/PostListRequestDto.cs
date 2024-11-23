namespace NATS.Services.Dtos.RequestDtos;

public class PostListRequestDto : IRequestDto<PostListRequestDto>
{
    public bool OrderByAscending { get; set; } = true;
    public int? Page { get; set; }
    public int? ResultsPerPage { get; set; }
    
    public PostListRequestDto TransformValues()
    {
        if (Page is null or 0)
        {
            Page = 1;
        }
        
        if (ResultsPerPage is null or 0)
        {
            ResultsPerPage = 15;
        }
        return this;
    }
}