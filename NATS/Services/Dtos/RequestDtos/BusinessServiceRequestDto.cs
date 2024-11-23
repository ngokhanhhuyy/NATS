namespace NATS.Services.Dtos.RequestDtos;

public class BusinessServiceRequestDto : IRequestDto<BusinessServiceRequestDto>
{
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Detail { get; set; }
    public string ThumbnailUrl { get; set; }
    public byte[] ThumbnailFile { get; set; }
    public bool ThumbnailChanged{ get; set; }
    public List<BusinessServiceFeatureRequestDto> Features { get; set; }
    public List<BusinessServicePhotoRequestDto> Photos { get; set; }

    public BusinessServiceRequestDto TransformValues()
    {
        Name = Name.ToNullIfEmpty();
        Summary = Summary.ToNullIfEmpty();
        Detail = Detail.ToNullIfEmpty();
        Features = Features?.Select(feature => feature.TransformValues()).ToList();
        Photos = Photos?.Select(photo => photo.TransformValues()).ToList();
        return this;
    }
}