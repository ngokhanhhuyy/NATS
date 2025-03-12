namespace NATS.Services.Dtos.ResponseDtos;

public class PostStatsResponseDto
{
    [Display(Name = DisplayNames.TotalPostCount)]
    public int TotalCount { get; set; }

    [Display(Name = DisplayNames.TotalPostViews)]
    public int TotalViews { get; set; }

    [Display(Name = DisplayNames.UnpublishedPostCount)]
    public int UnpublishedCount { get; set; }
}