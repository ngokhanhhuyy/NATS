namespace NATS.Services.Interfaces;

/// <summary>
/// A service to handle contacts-related operations.
/// </summary>
public interface IContactService
{
    /// <summary>
    /// Gets a list of all contacts.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a
    /// <see cref="List{T}"/> of DTOs, containing the information of the contacts.
    /// </returns>
    Task<List<ContactResponseDto>> GetListAsync();

    /// <summary>
    /// Updates an existing contact, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the contact to update.
    /// </param>
    /// <param name="requestDto">
    /// A DTO containing the data for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the contact with the specified id doesn't exist.
    /// </exception>
    Task UpdateAsync(int id, ContactUpsertRequestDto requestDto);
}