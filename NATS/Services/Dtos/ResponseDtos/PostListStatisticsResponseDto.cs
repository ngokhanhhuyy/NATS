namespace NATS.Services.Dtos.ResponseDtos;

public class PostListStatisticsResponseDto
{
    [Display(Name = DisplayNames.TotalPosts)]
    public int TotalPosts { get; set; }

    [Display(Name = DisplayNames.TotalViews)]
    public int TotalViews { get; set; }

    [Display(Name = DisplayNames.UnpublishedPosts)]
    public int UnpublishedPosts { get; set; }
}
