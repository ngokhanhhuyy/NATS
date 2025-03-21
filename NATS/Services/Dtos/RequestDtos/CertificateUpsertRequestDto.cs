namespace NATS.Services.Dtos.RequestDtos;

public class CertificateUpsertRequestDto : IHasThumbnailUpsertRequestDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] ThumbnailFile { get; set; }
    public bool ThumbnailChanged { get; set; } = false;

    public void TransformValues()
    {
        Name = Name.ToNullIfEmpty();
    }
}