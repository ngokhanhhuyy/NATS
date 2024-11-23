namespace NATS.Services;

public class ContactInfoService : IContactInfoService
{
    private readonly DatabaseContext _context;
    private readonly IValidator<ContactInfoRequestDto> _validator;
    
    public ContactInfoService(
            DatabaseContext context,
            IValidator<ContactInfoRequestDto> validator)
    {
        _context = context;
        _validator = validator;
    }
    
    public async Task<ServiceResult<ContactInfoResponseDto>> GetAsync()
    {
        // Fetch the entity from the database and map its data to the response dto.
        ContactInfoResponseDto responseDto = await _context.ContactInfos
            .Select(ci => new ContactInfoResponseDto
            {
                PhoneNumber = ci.PhoneNumber,
                ZaloNumber = ci.ZaloNumber,
                Email = ci.Email,
                Address = ci.Address
            }).SingleAsync();
        return ServiceResult<ContactInfoResponseDto>.Success(responseDto);
    }

    public async Task<ServiceResult<ContactInfoResponseDto>> UpdateAsync(ContactInfoRequestDto requestDto)
    {
        // Validate data from the request.
        ValidationResult result = _validator.Validate(requestDto.TransformValues());
        if (!result.IsValid)
        {
            return ServiceResult<ContactInfoResponseDto>.Failed(result.Errors);
        }

        // Fetch the entity from the database.
        ContactInfo contactInfo = await _context.ContactInfos.SingleAsync();

        // Perform update operation.
        contactInfo.PhoneNumber = requestDto.PhoneNumber;
        contactInfo.ZaloNumber = requestDto.ZaloNumber;
        contactInfo.Email = requestDto.Email;
        contactInfo.Address = requestDto.Address;

        // Save changes
        await _context.SaveChangesAsync();

        // Return the data of the updated entity to the response dto.
        ContactInfoResponseDto responseDto = new ContactInfoResponseDto
        {
            PhoneNumber = contactInfo.PhoneNumber,
            ZaloNumber = contactInfo.ZaloNumber,
            Email = contactInfo.Email,
            Address = contactInfo.Address
        };
        return ServiceResult<ContactInfoResponseDto>.Success(responseDto);
    }
}