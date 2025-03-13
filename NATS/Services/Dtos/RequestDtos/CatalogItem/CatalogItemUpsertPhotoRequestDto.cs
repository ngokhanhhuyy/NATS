namespace NATS.Services.Dtos.RequestDtos;

public class CatalogItemUpsertPhotoRequestDto : IRequestDto
{
    public int? Id { get; set; }
    public byte[] File { get; set; }
    public bool IsDeleted { get; set; }

    public void TransformValues()
    {
        Id = Id == 0 ? null : Id;
    }
}
