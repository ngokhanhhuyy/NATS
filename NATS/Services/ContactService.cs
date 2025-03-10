namespace NATS.Services;

/// <inheritdoc />
public class ContactService : IContactService
{
    private readonly DatabaseContext _context;
    
    public ContactService(DatabaseContext context)
    {
        _context = context;
    }
    
    /// <inheritdoc />
    public async Task<List<ContactResponseDto>> GetListAsync()
    {
        // Fetch a list of all entities from the database.
        return await _context.Contacts
            .Select(contact => new ContactResponseDto(contact))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, ContactUpsertRequestDto requestDto)
    {
        // Fetch the entity from the database and ensure it exists.
        Contact contact = await _context.Contacts
            .SingleOrDefaultAsync(contact => contact.Id == id)
            ?? throw new ResourceNotFoundException(nameof(Contact), nameof(id), id.ToString());

        // Perform update operation.
        contact.Type = requestDto.Type;
        contact.Content = requestDto.Content;

        // Save changes.
        await _context.SaveChangesAsync();
    }
}