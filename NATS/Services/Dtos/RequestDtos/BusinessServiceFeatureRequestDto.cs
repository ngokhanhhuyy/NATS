namespace NATS.Services.Dtos.RequestDtos;

public class BusinessServiceFeatureRequestDto : IRequestDto<BusinessServiceFeatureRequestDto>
{
    public int? Id { get; set; }
    public string Content { get; set; }
    public bool IsDeleted { get; set; }

    public BusinessServiceFeatureRequestDto TransformValues()
    {
        Content = Content.ToNullIfEmpty();
        return this;
    }
}