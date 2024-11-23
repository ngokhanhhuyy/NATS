namespace NATS.Services.Dtos.RequestDtos;

public class IntroductionItemRequestDto : IRequestDto<IntroductionItemRequestDto>
{
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Content { get; set; }
    public byte[] ThumbnailFile { get; set; }
    public bool ThumbnailChanged { get; set; }

    public IntroductionItemRequestDto TransformValues()
    {
        Name = Name.ToNullIfEmpty();
        Summary = Summary.ToNullIfEmpty();
        Content = Content.ToNullIfEmpty();
        return this;
    }
}
