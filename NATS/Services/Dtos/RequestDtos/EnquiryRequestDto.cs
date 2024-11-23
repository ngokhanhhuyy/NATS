namespace NATS.Services.Dtos.RequestDtos;

public class EnquiryRequestDto : IRequestDto<EnquiryRequestDto>
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Content { get; set; }
    
    public EnquiryRequestDto TransformValues()
    {
        FullName = FullName.ToNullIfEmpty();
        PhoneNumber = PhoneNumber.ToNullIfEmpty();
        Email = Email.ToNullIfEmpty();
        Content = Content.ToNullIfEmpty();
        return this;
    }
}