namespace NATS.Services.Interfaces;

/// <summary>
/// A service to handle the operations which are related to slider items.
/// </summary>
public interface IHomePageSliderItemService
{
    /// <summary>
    /// Gets a list of all slider items.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a
    /// <see cref="List{T}"/> of DTOs containing the information of the slider items.
    /// </returns>
    Task<List<SliderItemResponseDto>> GetListAsync();

    /// <summary>
    /// Gets a single existing slider item, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the slider item to retrieves.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a DTO
    /// containing the information of the slider item.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the slider item specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    Task<SliderItemResponseDto> GetSingleAsync(int id);

    /// <summary>
    /// Creates a new slider item.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the data for the creating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a
    /// the id of the created slider item.
    /// </returns>
    Task<int> CreateAsync(SliderItemUpsertRequestDto requestDto);

    /// <summary>
    /// Updates an existing slider item, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the slider item to update.
    /// </param>
    /// <param name="requestDto">
    /// A DTO containing the data for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency-related conflict occuring during the operation.
    /// </exception>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the slider item specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    Task UpdateAsync(int id, SliderItemUpsertRequestDto requestDto);

    /// <summary>
    /// Deletes an existing slider item, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the slider item to delete.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency-related conflict occuring during the operation.
    /// </exception>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the slider item specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    Task DeleteAsync(int id);
}