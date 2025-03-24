namespace NATS.FrontPages.Models;

public class EnquiryViewModel
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Content { get; set; }
    public bool IsValidated { get; set; }

    public EnquiryCreateRequestDto ToRequestDto()
    {
        EnquiryCreateRequestDto requestDto = new EnquiryCreateRequestDto
        {
            FullName = FullName,
            PhoneNumber = PhoneNumber,
            Email = Email,
            Content = Content
        };

        requestDto.TransformValues();

        return requestDto;
    }
}