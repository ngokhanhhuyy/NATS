namespace NATS.Services.Dtos.RequestDtos;

public class ProductPhotoRequestDto : IRequestDto<ProductPhotoRequestDto>
{
    public int? Id { get; set; }
    public byte[] File { get; set; }
    public bool IsDeleted { get; set; }

    public ProductPhotoRequestDto TransformValues()
    {
        Id = Id == 0 ? null : Id;
        return this;
    }
}