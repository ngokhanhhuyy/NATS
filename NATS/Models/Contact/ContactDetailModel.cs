namespace NATS.Models;

public class ContactDetailModel
{
    [Display(Name = DisplayNames.Id)]
    public int Id { get; set; }
    
    [Display(Name = DisplayNames.Type)]
    public ContactType Type { get; set; }
    
    [Display(Name = DisplayNames.Content)]
    public string Content { get; set; }

    public ContactDetailModel(ContactResponseDto responseDto)
    {
        Id = responseDto.Id;
        Type = responseDto.Type;
        Content = responseDto.Content;
    }
}