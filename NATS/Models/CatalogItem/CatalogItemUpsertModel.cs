namespace NATS.Models;

public class CatalogItemUpsertModel
{
    [Display(Name = DisplayNames.Id)]
    public int Id { get; set; }

    [Display(Name = DisplayNames.Name)]
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Display(Name = DisplayNames.Type)]
    [Required]
    public CatalogItemType Type { get; set; }

    [Display(Name = DisplayNames.Summary)]
    [MaxLength(255)]
    public string Summary { get; set; }

    [Display(Name = DisplayNames.Detail)]
    [MaxLength(5000)]
    public string Detail { get; set; }

    [Display(Name = DisplayNames.Thumbnail)]
    public string ThumbnailUrl { get; set; }

    [Display(Name = DisplayNames.ThumbnailFile)]
    public IFormFile ThumbnailFile { get; set; }

    public bool ThumbnailChanged { get; set; }

    [Display(Name = DisplayNames.Photo)]
    public List<CatalogItemUpsertPhotoModel> Photos { get; set; }

    [BindNever]
    public bool IsForCreating { get; set; } = false;

    public CatalogItemUpsertModel() { }

    public CatalogItemUpsertModel(CatalogItem item)
    {
        Id = item.Id;
        Name = item.Name;
        Summary = item.Summary;
        Detail = item.Detail;
        ThumbnailUrl = item.ThumbnailUrl;
    }

    public void RemoveUnnecessaryPhotos()
    {
        if (Photos != null)
        {
            Photos = Photos.Where(photo => !photo.Id.HasValue && photo.IsDeleted).ToList();
        }
    }

    public async Task<CatalogItemUpsertRequestDto> ToRequestDto()
    {
        byte[] thumbnailFile = null;
        if (ThumbnailFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await ThumbnailFile.CopyToAsync(stream);
            thumbnailFile = stream.ToArray();
        }

        List<CatalogItemUpsertPhotoRequestDto> photoRequestDtos;
        photoRequestDtos = new List<CatalogItemUpsertPhotoRequestDto>();
        foreach (CatalogItemUpsertPhotoModel photo in Photos)
        {
            photoRequestDtos.Add(await photo.ToRequestDto());
        }

        return new CatalogItemUpsertRequestDto {
            Name = Name,
            Summary = Summary,
            Detail = Detail,
            ThumbnailUrl = ThumbnailUrl,
            ThumbnailFile = thumbnailFile,
            ThumbnailChanged = ThumbnailChanged,
            Photos = photoRequestDtos
        };
    }
}