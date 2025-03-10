namespace NATS.Services.Interfaces;

/// <summary>
/// A service to handle operations which are related to team members.
/// </summary>
public interface ITeamMemberService
{
    /// <summary>
    /// Gets a list of all team members.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a
    /// <see cref="List{T}"/> of DTOs containing the information of the team members.
    /// </returns>
    Task<List<TeamMemberResponseDto>> GetListAsync();

    /// <summary>
    /// Gets a single existing team member, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the team member to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a DTO
    /// containing the information of the team member.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the team member specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    Task<TeamMemberResponseDto> GetSingleAsync(int id);

    /// <summary>
    /// Creates a new team member.
    /// </summary>
    /// <param name="upsertRequestDto">
    /// A DTO containing the data for the creating operation.
    /// </param>
    /// <returns>
    /// The id of the created team member.
    /// </returns>
    Task<int> CreateAsync(TeamMemberUpsertRequestDto upsertRequestDto);

    /// <summary>
    /// Updates an existing team member, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the team member to update.
    /// </param>
    /// <param name="upsertRequestDto">
    /// A DTO containing the data for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency-related conflict occuring during the operation.
    /// </exception>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the team member specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    Task UpdateAsync(int id, TeamMemberUpsertRequestDto upsertRequestDto);

    /// <summary>
    /// Deletes an existing team member, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the team member to delete.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency-related conflict occuring during the operation.
    /// </exception>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the team member specified by <paramref name="id"/> doesn't exist.
    /// </exception>
    Task DeleteAsync(int id);
}