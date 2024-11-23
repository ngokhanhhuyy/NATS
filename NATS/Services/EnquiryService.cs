using Microsoft.EntityFrameworkCore.Storage;

namespace NATS.Services;

public class EnquiryService : IEnquiryService
{
    private readonly DatabaseContext _context;
    private readonly IValidator<EnquiryRequestDto> _validator;
    
    public EnquiryService(DatabaseContext context, IValidator<EnquiryRequestDto> validator)
    {
        _context = context;
        _validator = validator;
    }
    
    /// <summary>
    /// Get a list of all enquiries.
    /// </summary>
    /// <returns>A list of objects containing the data of the enquiries.</returns>
    public async Task<ServiceResult<List<EnquiryResponseDto>>> GetListAsync()
    {
        // Fetch a list of entities in the database, then map to response dtos.
        List<EnquiryResponseDto> responseDtos = await _context.Enquiries
            .Select(e => new EnquiryResponseDto
            {
                Id = e.Id,
                FullName = e.FullName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                Content = e.Content,
                ReceivedDateTime = e.ReceivedDateTime,
                IsCompleted = e.IsCompleted
            }).ToListAsync();
        
        // Return the response dtos.
        return ServiceResult<List<EnquiryResponseDto>>.Success(responseDtos);
    }
    
    /// <summary>
    /// Get an enquiry by given id.
    /// </summary>
    /// <param name="id">The id of the enquiry.</param>
    /// <returns>An object containing the information of the enquiry.</returns>
    public async Task<ServiceResult<EnquiryResponseDto>> GetAsync(int id)
    {
        // Fetch the entity with given id from the database and map to response dto.
        EnquiryResponseDto responseDto = await _context.Enquiries
            .Where(e => e.Id == id)
            .Select(e => new EnquiryResponseDto
            {
                Id = e.Id,
                FullName = e.FullName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                Content = e.Content,
                ReceivedDateTime = e.ReceivedDateTime,
                IsCompleted = e.IsCompleted
            }).SingleOrDefaultAsync();
        
        // Ensure the entity exists in the database.
        if (responseDto == null)
        {
            return ServiceResult<EnquiryResponseDto>.Failed(
                ServiceError.NotFoundByProperty(
                    nameof(Enquiry),
                    nameof(id),
                    id.ToString()));
        }
        
        // Return the response dto
        return ServiceResult<EnquiryResponseDto>.Success(responseDto);
    }
    
    /// <summary>
    /// Get the number of enquiries that has not been completed yet.
    /// </summary>
    /// <returns>The number of incompleted enquiries.</returns>
    public async Task<ServiceResult<int>> GetIncompletedCountAsync()
    {
        int enquiryCount = await _context.Enquiries
            .CountAsync(e => !e.IsCompleted);
        return ServiceResult<int>.Success(enquiryCount);
    }
    
    /// <summary>
    /// Create an enquiry with given data for a new one.
    /// </summary>
    /// <param name="requestDto">An object containing the data for a new enquiry.</param>
    /// <returns>The id of the created enquiry.</returns>
    public async Task<ServiceResult<int>> CreateAsync(EnquiryRequestDto requestDto)
    {
        // Validate the data from the request.
        ValidationResult result = _validator.Validate(requestDto.TransformValues());
        if (!result.IsValid)
        {
            return ServiceResult<int>.Failed(result.Errors);
        }
        
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
        
        // Return the id of the created entity.
        return ServiceResult<int>.Success(enquiry.Id);
    }
    
    /// <summary>
    /// Mark an enquiry by given id as completed.
    /// </summary>
    /// <param name="id">The id of the enquiry.</param>
    /// <returns>The id of the updated enquiry.</returns>
    public async Task<ServiceResult<int>> MarkAsCompletedAsync(int id)
    {
        // Use transaction to ensure the update operation only affects when exactly one entity with
        // given id is found.
        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();
        
        // Perform the update operation on the entity with given id.
        int affectedEntities = await _context.Enquiries
            .Where(e => e.Id == id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.IsCompleted, true));
        
        // Ensure that exactly one entity has been affected.
        if (affectedEntities != 1)
        {
            await transaction.RollbackAsync();
            return ServiceResult<int>.Failed(ServiceError.NotFoundByProperty(
                nameof(Enquiry),
                nameof(id),
                id.ToString()));
        }
        
        // Commit the transaction and return the id of the updated entity.
        await transaction.CommitAsync();
        return ServiceResult<int>.Success(id);
    }
}