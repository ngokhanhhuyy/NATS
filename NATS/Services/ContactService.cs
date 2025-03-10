namespace NATS.Services;

public class ContactService : IContactInfoService
{
    private readonly DatabaseContext _context;
    
    public ContactService(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<List<ContactResponseDto>> GetListAsync()
    {
        // Fetch a list of all entities from the database.
        return await _context.Contacts
            .Select(contact => new ContactResponseDto(contact))
            .ToListAsync();
    }

    public async Task<ServiceResult<ContactResponseDto>> UpdateAsync(ContactUpsertRequestDto requestDto)
    {
        // Validate data from the request.
        ValidationResult result = _validator.Validate(requestDto.TransformValues());
        if (!result.IsValid)
        {
            return ServiceResult<ContactResponseDto>.Failed(result.Errors);
        }

        // Fetch the entity from the database.
        Contact contactInfo = await _context.Contacts.SingleAsync();

        // Perform update operation.
        contactInfo.PhoneNumber = requestDto.PhoneNumber;
        contactInfo.ZaloNumber = requestDto.ZaloNumber;
        contactInfo.Email = requestDto.Email;
        contactInfo.Address = requestDto.Address;

        // Save changes
        await _context.SaveChangesAsync();

        // Return the data of the updated entity to the response dto.
        ContactResponseDto responseDto = new ContactResponseDto
        {
            PhoneNumber = contactInfo.PhoneNumber,
            ZaloNumber = contactInfo.ZaloNumber,
            Email = contactInfo.Email,
            Address = contactInfo.Address
        };
        return ServiceResult<ContactResponseDto>.Success(responseDto);
    }
}