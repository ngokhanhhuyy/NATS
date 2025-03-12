namespace NATS.Models;

public class ContactUpsertModel
{
    [Display(Name = DisplayNames.Id)]
    public int Id { get; set; }
    
    [Display(Name = DisplayNames.Type)]
    public ContactType Type { get; set; }
    
    [Display(Name = DisplayNames.Content)]
    [MaxLength(255)]
    public string Content { get; set; }

    public ContactUpsertModel() { }

    public ContactUpsertModel(ContactResponseDto responseDto)
    {
        Id = responseDto.Id;
        Type = responseDto.Type;
        Content = responseDto.Content;
    }
}