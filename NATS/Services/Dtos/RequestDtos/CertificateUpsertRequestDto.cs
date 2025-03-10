namespace NATS.Services.Dtos.RequestDtos;

public class BusinessCertificateRequestDto : IRequestDto<BusinessCertificateRequestDto>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhotoUrl { get; set; }
    public byte[] PhotoFile { get; set; }
    public bool PhotoChanged = false;

    public BusinessCertificateRequestDto TransformValues()
    {
        Name = Name.ToNullIfEmpty();
        return this;
    }
}