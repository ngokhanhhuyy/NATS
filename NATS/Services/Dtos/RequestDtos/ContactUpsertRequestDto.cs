namespace NATS.Services.Dtos.RequestDtos;

public class ContactUpsertRequestDto : IRequestDto<ContactUpsertRequestDto>
{
    public ContactType Type { get; set; }
    public string Content { get; set; }

    public ContactUpsertRequestDto TransformValues()
    {
        Content = Content.ToNullIfEmpty();
        return this;
    }
}