namespace NATS.Services.Dtos.RequestDtos;

public class CourseRequestDto : IRequestDto<CourseRequestDto>
{
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Detail { get; set; }
    public string ThumbnailUrl { get; set; }
    public byte[] ThumbnailFile { get; set; }
    public bool ThumbnailChanged { get; set; }
    public List<CourseSectionRequestDto> Sections { get; set; }
    public List<CoursePhotoRequestDto> Photos { get; set; }

    public CourseRequestDto TransformValues()
    {
        Name = Name.ToNullIfEmpty();
        Summary = Summary.ToNullIfEmpty();
        Detail = Detail.ToNullIfEmpty();
        Sections = Sections?.Select(section => section.TransformValues()).ToList();
        Photos = Photos?.Select(photo => photo.TransformValues()).ToList();
        return this;
    }
}
