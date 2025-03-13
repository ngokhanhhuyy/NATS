namespace NATS.Services.Dtos.RequestDtos;

public class ContactUpsertRequestDto : IRequestDto
{
    public ContactType Type { get; set; }
    public string Content { get; set; }

    public void TransformValues()
    {
        Content = Content.ToNullIfEmpty();
    }
}