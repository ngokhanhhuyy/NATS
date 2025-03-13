namespace NATS.Services.Interfaces;

/// <summary>
/// A service to handle the operations which are related to authorization.
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Gets the id of the requesting user.
    /// </summary>
    /// <returns>
    /// An <see cref="int"/> value representing the id of the requesting user.
    /// </returns>
    int GetUserId();
    
    /// <summary>
    /// Retrieves the details of the requesting/caller user.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is an 
    /// instance of the <see cref="UserDetailResponseDto"/> class, containing the details of
    /// the user.
    /// </returns>
    Task<UserDetailResponseDto> GetCallerUserDetailAsync();

    /// <summary>
    /// Checks if the caller user has permission to reset the target user's password.
    /// </summary>
    /// <param name="targetUser">
    /// An entity representing the target user to compare.
    ///  </param>
    /// <returns>
    /// A <see cref="bool"/> value indicating whether the caller has the permission.
    /// </returns>
    bool CanResetUserPassword(User targetUser);
}