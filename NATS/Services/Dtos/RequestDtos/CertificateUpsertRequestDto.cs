namespace NATS.Services.Dtos.RequestDtos;

public class CertificateUpsertRequestDto : IHasThumbnailUpsertRequestDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ThumbnailUrl { get; set; }

    public void TransformValues()
    {
        Name = Name.ToNullIfEmpty();
        ThumbnailUrl = ThumbnailUrl.ToNullIfEmpty();
    }
}