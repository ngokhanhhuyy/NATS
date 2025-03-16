namespace NATS.Services.Interfaces;

/// <summary>
/// A service to handle operations which are related to catalog items.
/// </summary>
public interface ICatalogItemService
{
    /// <summary>
    /// Get the list of all catalog items with basic information and thumbnail only.
    /// </summary>
    /// <param name="type">
    /// (Optional) The type of the catalog items to retrieve. If not specified, the results
    /// will be all available catalog items, regardless their types.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a
    /// <see cref="List{T}"/> of DTOs, containing the basic information of the catalog items.
    /// </returns>
    Task<List<CatalogItemBasicResponseDto>> GetListAsync(CatalogItemType? type = null);

    /// <summary>
    /// Gets a specific catalog item by given id with detail information, thumbnail url and
    /// photos.
    /// </summary>
    /// <param name="id">
    /// The id of the catalog item to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a DTO
    /// containing the information of the item.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the catalog item with the given id doesn't exist.
    /// </exception>
    Task<CatalogItemDetailResponseDto> GetDetailAsync(int id);

    /// <summary>
    /// Create a new catalog item with the given data.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the data for the creating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the
    /// id of the created catalog item.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    Task<int> CreateAsync(CatalogItemUpsertRequestDto requestDto);

    /// <summary>
    /// Update an existing catalog item by the given id with the data specified in the request,
    /// containing thumbnail and photo files.
    /// </summary>
    /// <param name="id">
    /// The id of the catalog item to be updated.
    /// </param>
    /// <param name="requestDto">
    /// A DTO containing the data for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when one or many photos with the specified ids in the
    /// <paramref name="requestDto"/> don't exist in the database.
    /// </exception>
    Task UpdateAsync(int id, CatalogItemUpsertRequestDto requestDto);

    /// <summary>
    /// Delete an existing catalog item with the given id, including its thumbnail and photos.
    /// </summary>
    /// <param name="id">
    /// The id of the catalog item to be deleted.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> reprensenting the asynchronous operation.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    Task DeleteAsync(int id);
}