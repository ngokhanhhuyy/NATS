namespace NATS.Models;

public class CatalogItemUpsertPhotoModel
{
    [Display(Name = DisplayNames.Id)]
    public int? Id { get; set; }

    [Display(Name = DisplayNames.Photo)]
    public string Url { get; set; }

    [Display(Name = DisplayNames.PhotoFile)]
    public IFormFile File { get; set; }

    public bool IsDeleted { get; set; } = false;
    
    public CatalogItemUpsertPhotoModel() { }

    public CatalogItemUpsertPhotoModel(CatalogItemDetailPhotoResponseDto photo)
    {
        Id = photo.Id;
        Url = photo.Url;
    }

    public async Task<CatalogItemUpsertPhotoRequestDto> ToRequestDto()
    {
        byte[] file = null;
        if (File != null)
        {
            using MemoryStream stream = new MemoryStream();
            await File.CopyToAsync(stream);
            file = stream.ToArray();
        }
        
        return new CatalogItemUpsertPhotoRequestDto
        {
            File = file,
            IsDeleted = IsDeleted
        };
    }
}