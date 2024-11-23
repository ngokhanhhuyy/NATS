namespace NATS.Services.Dtos.RequestDtos;

public class ProductRequestDto : IRequestDto<ProductRequestDto>
{
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Detail { get; set; }
    public string ThumbnailUrl { get; set; }
    public byte[] ThumbnailFile { get; set; }
    public bool ThumbnailChanged { get; set; }
    public List<ProductFeatureRequestDto> Features { get; set; }
    public List<ProductPhotoRequestDto> Photos { get; set; }

    public ProductRequestDto TransformValues()
    {
        Name = Name.ToNullIfEmpty();
        Summary = Summary.ToNullIfEmpty();
        Detail = Detail.ToNullIfEmpty();
        Features = Features?.Select(feature => feature.TransformValues()).ToList();
        Photos = Photos?.Select(photo => photo.TransformValues()).ToList();
        return this;
    }
}