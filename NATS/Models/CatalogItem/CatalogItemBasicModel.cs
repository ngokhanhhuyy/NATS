namespace NATS.Models;

public class CatalogItemBasicModel
{
    [Display(Name = DisplayNames.Id)]
    public int Id { get; set; }

    [Display(Name = DisplayNames.Name)]
    public string Name { get; set; }

    [Display(Name = DisplayNames.Type)]
    public CatalogItem Type { get; set; }

    [Display(Name = DisplayNames.Summary)]
    public string Summary { get; set; }

    [Display(Name = DisplayNames.Thumbnail)]
    public string ThumbnailUrl { get; set; }

    public CatalogItemBasicModel(CatalogItemBasicResponseDto responseDto)
    {
        Id = responseDto.Id;
        Name = responseDto.Name;
        Summary = responseDto.Summary;
        ThumbnailUrl = responseDto.ThumbnailUrl;
    }
}