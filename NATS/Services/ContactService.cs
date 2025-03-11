namespace NATS.Services;

/// <inheritdoc cref="IContactService"/>
public class ContactService
       :
            AbstractUpsertableService<Contact, ContactUpsertRequestDto>,
            IContactService
{
    
    public ContactService(DatabaseContext context) : base(context)
    {
    }
    
    /// <inheritdoc />
    public async Task<List<ContactResponseDto>> GetListAsync()
    {
        return await Context.Contacts
            .Select(contact => new ContactResponseDto(contact))
            .ToListAsync();
    }


    /// <inheritdoc />
    public async Task<int> CreateAsync(ContactUpsertRequestDto requestDto)
    {
        Contact contact = new Contact
        {
            Type = requestDto.Type,
            Content = requestDto.Content
        };

        return await base.SaveCreatedEntityAsync(contact, requestDto);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, ContactUpsertRequestDto requestDto)
    {
        // Fetch the entity from the database and ensure it exists.
        Contact contact = await Context.Contacts
            .SingleOrDefaultAsync(contact => contact.Id == id)
            ?? throw new ResourceNotFoundException(nameof(Contact), nameof(id), id.ToString());

        // Perform update operation.
        contact.Type = requestDto.Type;
        contact.Content = requestDto.Content;

        // Save changes.
        await base.SaveUpdatedEntityAsync(contact, requestDto);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        // Fetch the entity from the database and ensure it exists.
        Contact contact = await Context.Contacts
            .SingleOrDefaultAsync(contact => contact.Id == id)
            ?? throw new ResourceNotFoundException(nameof(Contact), nameof(id), id.ToString());

        await base.SaveDeletedEntityAsync(contact);
    }

    /// <inheritdoc />
    protected override sealed DbSet<Contact> GetRepository(DatabaseContext context)
    {
        return context.Contacts;
    }
}