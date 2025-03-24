namespace NATS.Services.Interfaces;

/// <summary>
/// A service to handle enquiries-related operations.
/// </summary>
public interface IEnquiryService
{
    /// <summary>
    /// Gets a list of all enquiries.
    /// </summary>
    /// <returns>
    /// A list of objects containing the data of the enquiries.
    /// </returns>
    Task<List<EnquiryResponseDto>> GetListAsync();

    /// <summary>
    /// Gets a single existing enquiry by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the enquiry to retrieve.
    /// </param>
    /// <returns>
    /// The number of incompleted enquiries.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the enquiry specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    Task<EnquiryResponseDto> GetSingleAsync(int id);

    /// <summary>
    /// Gets the number of enquiries that has not been completed yet.
    /// </summary>
    /// <returns>
    /// The number of incompleted enquiries.
    /// </returns>
    Task<int> GetIncompletedCountAsync();

    /// <summary>
    /// Creates an enquiry with given data for a new one.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the data for the creating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the id
    /// of the created enquiry.
    /// </returns>
    Task<int> CreateAsync(EnquiryCreateRequestDto requestDto);

    /// <summary>
    /// Marks an existing enquiry, specified by id, as completed.
    /// </summary>
    /// <param name="id">
    /// The id of the enquiry to mark.
    /// </param>
    /// <returns>
    /// The id of the updated enquiry.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the enquiry with the specified <paramref name="id"/> doesn't exist.
    /// </exception>
    Task MarkAsCompletedAsync(int id);
}