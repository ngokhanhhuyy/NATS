namespace NATS.Services.Interfaces;

/// <summary>
/// A service to handle operations which are related to summary items.
/// </summary>
public interface ISummaryItemService
{
    /// <summary>
    /// Gets a list of all summary items.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a
    /// <see cref="List{T}"/> of DTOs containing the information of the summary items.
    /// </returns>
    Task<List<SummaryItemResponseDto>> GetListAsync();

    /// <summary>
    /// Gets a single existing summary item, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the summary id to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a DTO
    /// containing the information of the summary items.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the summary item, specified by <paramref name="id"/>, doesn't exist.
    /// </exception>
    Task<SummaryItemResponseDto> GetSingleAsync(int id);

    /// <summary>
    /// Updates an existing summary item, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the summary item to update.
    /// </param>
    /// <param name="requestDto">
    /// A DTO containing the data for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the summary item, specified by <paramref name="id"/>, doesn't exist.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency-related conflict occuring during the operation.
    /// </exception>
    Task UpdateAsync(int id, SummaryItemUpsertRequestDto requestDto);
}