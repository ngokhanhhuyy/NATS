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
    /// Gets a single contact.
    /// </summary>
    /// <param name="id">
    /// The id of the contact to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a DTO
    /// containing the information of the contact.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the contact specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    Task<ContactResponseDto> GetSingleAsync(int id);

    /// <summary>
    /// Creates a new contact.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the data for the creating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{Task}"/> representing the asynchronous operation, which result is the
    /// id of the created contact.
    /// </returns>
    Task<int> CreateAsync(ContactUpsertRequestDto requestDto);

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
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency-related conflict occuring during the operation.
    /// </exception>
    Task UpdateAsync(int id, ContactUpsertRequestDto requestDto);

    /// <summary>
    /// Deletes an existing contact.
    /// </summary>
    /// <param name="id">
    /// The id of the contact to delete.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the contact with the specified id doesn't exist.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency-related conflict occuring during the operation.
    /// </exception>
    Task DeleteAsync(int id);
}