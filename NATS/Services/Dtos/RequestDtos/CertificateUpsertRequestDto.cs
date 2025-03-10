namespace NATS.Services.Dtos.RequestDtos;

public class CertificateUpsertRequestDto : IRequestDto<CertificateUpsertRequestDto>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhotoUrl { get; set; }
    public byte[] PhotoFile { get; set; }
    public bool PhotoChanged = false;

    public CertificateUpsertRequestDto TransformValues()
    {
        Name = Name.ToNullIfEmpty();
        return this;
    }
}