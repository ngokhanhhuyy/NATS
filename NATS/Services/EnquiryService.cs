namespace NATS.Services;

/// <inheritdoc/>
public class EnquiryService : IEnquiryService
{
    private readonly DatabaseContext _context;
    
    public EnquiryService(DatabaseContext context, IValidator<EnquiryUpsertRequestDto> validator)
    {
        _context = context;
        _validator = validator;
    }

    /// <inheritdoc/>
    public async Task<List<EnquiryResponseDto>> GetListAsync()
    {
        return await _context.Enquiries
            .Select(e => new EnquiryResponseDto(e))
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<EnquiryResponseDto> GetSingleAsync(int id)
    {
        return await _context.Enquiries
            .Where(e => e.Id == id)
            .Select(e => new EnquiryResponseDto(e))
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException(nameof(Enquiry), nameof(id), id.ToString());
    }

    /// <inheritdoc/>
    public async Task<int> GetIncompletedCountAsync()
    {
        return await _context.Enquiries.CountAsync(e => !e.IsCompleted);
    }

    /// <inheritdoc/>
    public async Task<int> CreateAsync(EnquiryUpsertRequestDto requestDto)
    {
        // Initialize the entity.
        Enquiry enquiry = new Enquiry
        {
            FullName = requestDto.FullName,
            Email = requestDto.Email,
            PhoneNumber = requestDto.PhoneNumber,
            Content = requestDto.Content
        };

        _context.Enquiries.Add(enquiry);
        
        // Save changes.
        await _context.SaveChangesAsync();

        return enquiry.Id;
    }
    
    /// <summary>
    /// Mark an enquiry by given id as completed.
    /// </summary>
    /// <param name="id">The id of the enquiry.</param>
    /// <returns>The id of the updated enquiry.</returns>
    public async Task MarkAsCompletedAsync(int id)
    {
        // Use transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context
            .Database
            .BeginTransactionAsync();
        
        // Perform the update operation on the entity with given id.
        int affectedEntities = await _context.Enquiries
            .Where(e => e.Id == id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.IsCompleted, true));
        
        // Ensure that exactly one entity has been affected.
        if (affectedEntities != 1)
        {
            throw new ResourceNotFoundException(nameof(Enquiry), nameof(id), id.ToString());
        }
        
        // Commit the transaction and return the id of the updated entity.
        await transaction.CommitAsync();
    }
}