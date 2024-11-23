namespace NATS.Services.Dtos.RequestDtos;

public class CourseSectionRequestDto : IRequestDto<CourseSectionRequestDto>
{
    public int? Id { get; set; }
    public string Content { get; set; }
    public bool IsDeleted { get; set; }

    public CourseSectionRequestDto TransformValues()
    {
        Id = Id == 0 ? null : Id;
        Content = Content.ToNullIfEmpty();
        return this;
    }
}
