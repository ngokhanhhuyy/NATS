namespace NATS.Models;

public class ContactModel
{
    [Display(Name = DisplayNames.Id)]
    public int Id { get; set; }
    
    [Display(Name = DisplayNames.Type)]
    public ContactType Type { get; set; }
    
    [Display(Name = DisplayNames.Content)]
    public string Content { get; set; }

    public ContactModel(ContactResponseDto responseDto)
    {
        Id = responseDto.Id;
        Type = responseDto.Type;
        Content = responseDto.Content;
    }
}