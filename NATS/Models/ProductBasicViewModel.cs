namespace NATS.Models;

public class ProductBasicViewModel
{
    [Display(Name = DisplayNames.Id)]
    public int Id { get; set; }
    
    [Display(Name = DisplayNames.Name)]
    public string Name { get; set; }
    
    [Display(Name = DisplayNames.Summary)]
    public string Summary { get; set; }
    
    [Display(Name = DisplayNames.Detail)]
    public string Detail { get; set; }
    
    [Display(Name = DisplayNames.Thumbnail)]
    public string ThumbnailUrl { get; set; }
}