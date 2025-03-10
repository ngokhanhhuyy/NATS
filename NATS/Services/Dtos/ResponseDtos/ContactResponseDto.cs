namespace NATS.Services.Dtos.ResponseDtos;

public class ContactResponseDto
{
    public int Id { get; set; }
    public ContactType Type { get; set; }
    public string Content { get; set; }

    public ContactResponseDto(Contact contact)
    {
        Id = contact.Id;
        Type = contact.Type;
        Content = contact.Content;
    }
}