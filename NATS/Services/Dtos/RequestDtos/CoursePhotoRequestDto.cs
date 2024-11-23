namespace NATS.Services.Dtos.RequestDtos;

public class CoursePhotoRequestDto : IRequestDto<CoursePhotoRequestDto>
{
    public int? Id { get; set; }
    public byte[] File { get; set; }
    public bool IsDeleted { get; set; }

    public CoursePhotoRequestDto TransformValues()
    {
        Id = Id == 0 ? null : Id;
        return this;
    }
}
