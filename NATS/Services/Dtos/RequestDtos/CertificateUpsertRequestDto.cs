namespace NATS.Services.Dtos.RequestDtos;

public class CertificateUpsertRequestDto
        : IHasThumbnailUpsertRequestDto<CertificateUpsertRequestDto>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhotoUrl { get; set; }
    public byte[] ThumbnailFile { get; set; }
    public bool ThumbnailChanged { get; set; } = false;

    public CertificateUpsertRequestDto TransformValues()
    {
        Name = Name.ToNullIfEmpty();
        return this;
    }
}