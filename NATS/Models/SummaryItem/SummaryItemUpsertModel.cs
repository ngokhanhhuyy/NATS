namespace NATS.Models;

public class SummaryItemUpsertModel
{
    [Display(Name = DisplayNames.Id)]
    public int Id { get; set; }

    [Display(Name = DisplayNames.Name)]
    [Required]
    [MaxLength(25)]
    public string Name { get; set; }

    [Display(Name = DisplayNames.Summary)]
    [Required]
    [MaxLength(255)]
    public string SummaryContent { get; set; }

    [Display(Name = DisplayNames.Content)]
    [Required]
    [MaxLength(3000)]
    public string DetailContent { get; set; }

    [Display(Name = DisplayNames.Thumbnail)]
    public string ThumbnailUrl { get; set; }

    [Display(Name = DisplayNames.Photo)]
    public IFormFile ThumbnailFile { get; set; }

    public bool ThumbnailChanged { get; set; } = false;

    public bool IsForCreating { get; set; } = true;

    public SummaryItemUpsertModel(SummaryItemResponseDto responseDto = null)
    {
        if (responseDto != null)
        {
            Id = responseDto.Id;
            Name = responseDto.Name;
            SummaryContent = responseDto.SummaryContent;
            DetailContent = responseDto.DetailContent;
            ThumbnailUrl = responseDto.ThumbnailUrl;
            IsForCreating = false;
        }
    }
}