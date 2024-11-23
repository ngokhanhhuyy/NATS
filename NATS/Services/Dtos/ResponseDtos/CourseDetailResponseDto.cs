namespace NATS.Services.Dtos.ResponseDtos;

public class CourseDetailResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Detail { get; set; }
    public string ThumbnailUrl { get; set; }
    public List<CourseSectionResponseDto> Sections { get; set; }
    public List<CoursePhotoResponseDto> Photos { get; set; }
}
