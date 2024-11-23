namespace NATS.Services.Dtos.RequestDtos;

public class ProductFeatureRequestDto : IRequestDto<ProductFeatureRequestDto>
{
    public int? Id { get; set; }
    public string Content { get; set; }
    public bool IsDeleted { get; set; }

    public ProductFeatureRequestDto TransformValues()
    {
        Id = Id == 0 ? null : Id;
        Content = Content.ToNullIfEmpty();
        return this;
    }
}