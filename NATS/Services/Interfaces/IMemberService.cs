namespace NATS.Services.Interfaces;

/// <summary>
/// A service to handle operations which are related to members.
/// </summary>
public interface IMemberService
{
    /// <summary>
    /// Gets a list of all members.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a
    /// <see cref="List{T}"/> of DTOs containing the information of the members.
    /// </returns>
    Task<List<MemberResponseDto>> GetListAsync();

    /// <summary>
    /// Gets a single existing member, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the member to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a DTO
    /// containing the information of the member.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the member specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    Task<MemberResponseDto> GetSingleAsync(int id);

    /// <summary>
    /// Creates a new member.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the data for the creating operation.
    /// </param>
    /// <returns>
    /// The id of the created member.
    /// </returns>
    Task<int> CreateAsync(MemberUpsertRequestDto requestDto);

    /// <summary>
    /// Updates an existing member, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the member to update.
    /// </param>
    /// <param name="requestDDto">
    /// A DTO containing the data for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the member specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency-related conflict occuring during the operation.
    /// </exception>
    Task UpdateAsync(int id, MemberUpsertRequestDto requestDDto);

    /// <summary>
    /// Deletes an existing member, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the member to delete.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the member specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency-related conflict occuring during the operation.
    /// </exception>
    Task DeleteAsync(int id);
}