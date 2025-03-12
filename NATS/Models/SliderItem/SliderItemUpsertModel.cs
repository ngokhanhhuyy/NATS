namespace NATS.Models;

public class SliderItemUpsertModel : IHasThumbnailUpsertModel
{
    [Display(Name = DisplayNames.Id)]
    public int? Id { get; set; }

    [Display(Name = DisplayNames.Title)]
    [MaxLength(100)]
    public string Title { get; set; }

    [Display(Name = DisplayNames.Photo)]
    public string ThumbnailUrl { get; set; }

    [Display(Name = DisplayNames.Photo)]
    public IFormFile ThumbnailFile { get; set; }

    public bool ThumbnailChanged { get; set; } = false;

    [BindNever]
    public bool IsForCreating { get; set; } = true;

    public SliderItemUpsertModel(SliderItemResponseDto responseDto = null)
    {
        if (responseDto != null)
        {
            Id = responseDto.Id;
            Title = responseDto.Title;
            ThumbnailUrl = responseDto.ThumbnailUrl;
            IsForCreating = false;
        }
    }
}