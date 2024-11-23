namespace NATS.Services.Dtos.ResponseDtos;

public class BusinessServiceDetailResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Detail { get; set; }
    public string ThumbnailUrl { get; set; }
    public List<BusinessServiceFeatureResponseDto> Features { get; set; }
    public List<BusinessServicePhotoResponseDto> Photos { get; set; }
}