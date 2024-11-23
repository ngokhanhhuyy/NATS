namespace NATS.Services.Dtos.RequestDtos;

public class ContactInfoRequestDto : IRequestDto<ContactInfoRequestDto>
{
    public string PhoneNumber { get; set; }
    public string ZaloNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }

    public ContactInfoRequestDto TransformValues()
    {
        PhoneNumber = PhoneNumber.ToNullIfEmpty();
        ZaloNumber = ZaloNumber.ToNullIfEmpty();
        Email = Email.ToNullIfEmpty();
        Address = Address.ToNullIfEmpty();
        return this;
    }
}