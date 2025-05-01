namespace NATS.Protected.Models;

public class SliderItemUpsertViewModel : IHasThumbnailUpsertViewModel
{
    [Display(Name = DisplayNames.Title)]
    public string Title { get; set; }

    [Display(Name = DisplayNames.Thumbnail)]
    public string ThumbnailUrl { get; set; }

    [Display(Name = "Trang tạo")]
    public bool IsForCreating { get; set; } = true;

    public void MapFromResponseDto(SliderItemResponseDto responseDto)
    {
        Title = responseDto.Title;
        ThumbnailUrl = responseDto.ThumbnailUrl;
        IsForCreating = false;
    }

    public SliderItemUpsertRequestDto ToRequestDto()
    {
        return new SliderItemUpsertRequestDto
        {
            Title = Title,
            ThumbnailUrl = ThumbnailUrl
        };
    }
}