namespace NATS.Models;

public class SliderItemDetailModel
{
    [Display(Name = DisplayNames.Id)]
    public int? Id { get; set; }

    [Display(Name = DisplayNames.Title)]
    [MaxLength(100)]
    public string Title { get; set; }

    [Display(Name = DisplayNames.Photo)]
    public string ThumbnailUrl { get; set; }

    public SliderItemDetailModel(SliderItemResponseDto responseDto)
{
        Id = responseDto.Id;
        Title = responseDto.Title;
        ThumbnailUrl = responseDto.ThumbnailUrl;
    }
}