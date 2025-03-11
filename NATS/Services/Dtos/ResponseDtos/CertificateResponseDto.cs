namespace NATS.Services.Dtos.ResponseDtos;

public class CertificateResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhotoUrl { get; set; }

    public CertificateResponseDto(Certificate certificate)
    {
        Id = certificate.Id;
        Name = certificate.Name;
        PhotoUrl = certificate.ThumbnailUrl;
    }
}