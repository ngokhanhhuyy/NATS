namespace NATS.Services.Dtos.ResponseDtos;

public class ProductDetailResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Detail { get; set; }
    public string ThumbnailUrl { get; set; }
    public List<ProductFeatureResponseDto> Features { get; set; }
    public List<ProductPhotoResponseDto> Photos { get; set; }
}