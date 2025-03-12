namespace NATS.Models;

public class PostListModel
{
    [Display(Name = DisplayNames.Post)]
    public List<PostBasicModel> Results { get; set; }

    [Display(Name = DisplayNames.Page)]
    [BindNever]
    public int Page { get; set; } = 1;
    
    [Display(Name = DisplayNames.PageCount)]
    [BindNever]
    public int PageCount { get; set; }

    public PostListModel() { }

    public PostListModel(PostListResponseDto responseDto)
    {
        MapFromResponseDto(responseDto);
    }

    public void MapFromResponseDto(PostListResponseDto responseDto)
    {
        Results = responseDto.Results.Select(dto => new PostBasicModel(dto)).ToList();
    }
}