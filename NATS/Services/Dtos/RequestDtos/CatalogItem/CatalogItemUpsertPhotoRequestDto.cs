namespace NATS.Services.Dtos.RequestDtos;

public class CatalogItemUpsertPhotoRequestDto : IRequestDto<CatalogItemUpsertPhotoRequestDto>
{
    public int? Id { get; set; }
    public byte[] File { get; set; }
    public bool IsDeleted { get; set; }

    public CatalogItemUpsertPhotoRequestDto TransformValues()
    {
        Id = Id == 0 ? null : Id;
        return this;
    }
}
