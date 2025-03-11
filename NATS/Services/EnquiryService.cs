namespace NATS.Services;

/// <inheritdoc/>
public class EnquiryService
    :
        AbstractUpsertableService<Enquiry, EnquiryUpsertRequestDto>,
        IEnquiryService
{
    
    public EnquiryService(DatabaseContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<List<EnquiryResponseDto>> GetListAsync()
    {
        return await Context.Enquiries
            .Select(e => new EnquiryResponseDto(e))
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<EnquiryResponseDto> GetSingleAsync(int id)
    {
        return await Context.Enquiries
            .Where(e => e.Id == id)
            .Select(e => new EnquiryResponseDto(e))
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException(nameof(Enquiry), nameof(id), id.ToString());
    }

    /// <inheritdoc/>
    public async Task<int> GetIncompletedCountAsync()
    {
        return await Context.Enquiries.CountAsync(e => !e.IsCompleted);
    }
    
    /// <inheritdoc/>
    public async Task<int> CreateAsync(EnquiryUpsertRequestDto requestDto)
    {
        Enquiry enquiry = new Enquiry
        {
            FullName = requestDto.FullName,
            Email = requestDto.Email,
            PhoneNumber = requestDto.PhoneNumber,
            Content = requestDto.Content
        };

        return await base.SaveCreatedEntityAsync(enquiry, requestDto);
    }

    /// <inheritdoc/>
    public async Task MarkAsCompletedAsync(int id)
    {
        // Use transaction for atomic operations.
        await using IDbContextTransaction transaction = await Context
            .Database
            .BeginTransactionAsync();
        
        // Perform the update operation on the entity with given id.
        int affectedEntities = await Context.Enquiries
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

    /// <inheritdoc/>
    protected override sealed DbSet<Enquiry> GetRepository(DatabaseContext context)
    {
        return context.Enquiries;
    }
}