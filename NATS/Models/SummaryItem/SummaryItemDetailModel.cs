namespace NATS.Models;

public class SummaryItemDetailModel
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

    public SummaryItemDetailModel(SummaryItemResponseDto responseDto)
    {
        Id = responseDto.Id;
        Name = responseDto.Name;
        SummaryContent = responseDto.SummaryContent;
        DetailContent = responseDto.DetailContent;
        ThumbnailUrl = responseDto.ThumbnailUrl;
    }
}