namespace NATS.FrontPages.Models;

public class EnquiryViewModel
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Content { get; set; }
    public List<ContactDetailModel> Contacts { get; set; }

    public EnquiryViewModel(List<ContactResponseDto> contactResponseDtos)
    {
        Contacts = contactResponseDtos
            .Select(dto => new ContactDetailModel(dto))
            .ToList();
    } 

    public EnquiryUpsertRequestDto ToRequestDto()
    {
        EnquiryUpsertRequestDto requestDto = new EnquiryUpsertRequestDto
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