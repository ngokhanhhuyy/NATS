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
    /// Get the details of a specific user, specified by the id of the user.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> representing the id of the user to retrieve.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="UserDetailResponseDto"/> class, containing the details
    /// of the user.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the user with the specified id doesn't exist or has already been deleted.
    /// </exception>
    Task<UserDetailResponseDto> GetDetailAsync(int id);
    
    /// <summary>
    /// Gets the number of all users.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the
    /// number of all users.
    /// </returns>
    Task<int> GetCountAsync();

    /// <summary>
    /// Retrieves a specific user's role details, specified by the user's id.
    /// </summary>
    /// <param name="id">An <see cref="int"/> representing the id of the user.</param>
    /// <returns>
    /// An instance of the <see cref="RoleResponseDto"/> class, containing the details of the
    /// role to retrieve.
    /// </returns>
    Task<RoleResponseDto> GetRoleAsync(int id);

    /// <summary>
    /// Changes the password of the requested user.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="UserPasswordChangeRequestDto"/> class, contaning the
    /// current password, the new password and the confirmation password for the operation.
    /// </param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// The requesting user and the user whose id is specified by the value of the `id`
    /// argument must be the same one.
    /// </remarks>
    /// <exception cref="AuthorizationException">
    /// Throws when the requesting user isn't the target user.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the current password, provided in the <c>requestDto</c> is incorrect.
    /// </exception>
    Task ChangePasswordAsync(UserPasswordChangeRequestDto requestDto);

    /// <summary>
    /// Resets the password of the user, specified by the id, without the need of providing
    /// the current password.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> representing the id of the target user.
    /// </param>
    /// <param name="requestDto">
    /// An instance of the <see cref="UserPasswordResetRequestDto"/> class, contanining the
    /// new password and the confirmation password for the operation.
    /// </param>
    /// <returns>A <see cref="Task"/> representing the operation.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the user with the specified id doesn't exist or has already been deleted.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Throws when the requesting user is actually the target user, or doesn't have enough
    /// permissions to reset the target user's password.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the specified new password's complexity doesn't meet the requirement.
    /// </exception>
    Task ResetPasswordAsync(int id, UserPasswordResetRequestDto requestDto);
}
