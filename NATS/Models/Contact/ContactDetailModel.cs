namespace NATS.Models;

public class ContactDetailModel
{
    public int Id { get; set; }
    public ContactType Type { get; set; }
    public string Content { get; set; }

    public ContactDetailModel(ContactResponseDto responseDto)
    {
        Id = responseDto.Id;
        Type = responseDto.Type;
        Content = responseDto.Content;
    }
}