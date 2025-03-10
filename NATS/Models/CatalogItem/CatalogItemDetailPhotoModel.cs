namespace NATS.Models;

public class CatalogItemDetailPhotoModel
{
    [Display(Name = DisplayNames.Id)]
    public int Id { get; set; }

    [Display(Name = DisplayNames.Photo)]
    public string Url { get; set; }

    public CatalogItemDetailPhotoModel(CatalogItemPhoto photo)
    {
        Id = photo.Id;
        Url = photo.Url;
    }
}