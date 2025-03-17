namespace NATS.Models;

public class CertificateDetailModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ThumbnailUrl { get; set; }

    public CertificateDetailModel(CertificateResponseDto responseDto)
    {
        Id = responseDto.Id;
        Name = responseDto.Name;
        ThumbnailUrl = responseDto.ThumbnailUrl;
    }
}
