namespace NATS.Models;

public class CertificateDetailModel
{
    [Display(Name = DisplayNames.Id)]
    public int Id { get; set; }
    
    [Display(Name = DisplayNames.Name)]
    [MaxLength(100)]
    public string Name { get; set; }

    [Display(Name = DisplayNames.Thumbnail)]
    public string ThumbnailUrl { get; set; }

    public CertificateDetailModel(CertificateResponseDto responseDto)
    {
        Id = responseDto.Id;
        Name = responseDto.Name;
        ThumbnailUrl = responseDto.PhotoUrl;
    }
}