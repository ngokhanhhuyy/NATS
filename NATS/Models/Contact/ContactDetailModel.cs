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

    public string IconClassName => _iconClassNames[Type];
    public string UriEncodedContent => Uri.EscapeDataString(Content);

    private static readonly Dictionary<ContactType, string> _iconClassNames = new()
    {
        { ContactType.PhoneNumber, "bi-telephone-fill" },
        { ContactType.ZaloNumber, "bi-stop-circle-fill" },
        { ContactType.Email, "bi-envelope-at-fill" },
        { ContactType.Address, "bi-geo-alt-fill" }
    };
}