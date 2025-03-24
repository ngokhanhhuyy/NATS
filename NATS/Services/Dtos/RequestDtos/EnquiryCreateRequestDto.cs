namespace NATS.Services.Dtos.RequestDtos;

public class EnquiryCreateRequestDto : IRequestDto
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Content { get; set; }
    
    public void TransformValues()
    {
        FullName = FullName.ToNullIfEmpty();
        PhoneNumber = PhoneNumber.ToNullIfEmpty();
        Email = Email.ToNullIfEmpty();
        Content = Content.ToNullIfEmpty();
    }
}