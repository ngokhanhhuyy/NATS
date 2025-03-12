namespace NATS.Models;

public class CertificateUpsertModel : IHasThumbnailUpsertModel
{
    [Display(Name = DisplayNames.Id)]
    public int Id { get; set; }
    
    [Display(Name = DisplayNames.Name)]
    [MaxLength(100)]
    public string Name { get; set; }

    [Display(Name = DisplayNames.Thumbnail)]
    public string ThumbnailUrl { get; set; }

    [Display(Name = DisplayNames.Thumbnail)]
    public IFormFile ThumbnailFile { get; set; }

    public bool ThumbnailChanged { get; set; } = false;

    [BindNever]
    public bool IsForCreating { get; set; } = true;

    public CertificateUpsertModel() { }

    public CertificateUpsertModel(CertificateResponseDto responseDto)
    {
        Id = responseDto.Id;
        Name = responseDto.Name;
        ThumbnailUrl = responseDto.PhotoUrl;
        IsForCreating = false;
    }
}