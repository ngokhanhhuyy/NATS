namespace NATS.Services.Dtos.ResponseDtos;

public class EnquiryResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Content { get; set; }
    public DateTime ReceivedDateTime { get; set; }
    public bool IsCompleted { get; set; }

    public EnquiryResponseDto(Enquiry enquiry)
    {
        Id = enquiry.Id;
        FullName = enquiry.FullName;
        Email = enquiry.Email;
        PhoneNumber = enquiry.PhoneNumber;
        Content = enquiry.Content;
        ReceivedDateTime = enquiry.ReceivedDateTime;
        IsCompleted = enquiry.IsCompleted;
    }
}