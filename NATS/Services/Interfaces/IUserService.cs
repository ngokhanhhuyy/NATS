namespace NATS.Services.Interfaces;

/// <summary>
/// A service to handle user-related operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets a list of users, specified by pagination conditions.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the conditions for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a DTO
    /// containing the information of the users and the additional information for pagination.
    /// </returns>
    Task<UserListResponseDto> GetListAsync(UserListRequestDto requestDto);
    
    /// <summary>
    /// Gets the number of all users.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the
    /// number of all users.
    /// </returns>
    Task<int> GetCountAsync();
}
