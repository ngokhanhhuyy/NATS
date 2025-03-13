namespace NATS.Services;

/// <inheritdoc />
public class AuthorizationService : IAuthorizationService
{
    private readonly DatabaseContext _context;
    private readonly int _userId;
    private User _user;

    public AuthorizationService(
            DatabaseContext context,
            IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        ClaimsPrincipal user = httpContextAccessor.HttpContext?.User;
        
        string userIdAsString = user?.FindFirstValue(ClaimTypes.NameIdentifier);
        bool parsable = int.TryParse(userIdAsString, out int userId);
        if (!parsable)
        {
            throw new AuthenticationException();
        }

        _userId = userId;
    }
    
    /// <inheritdoc />
    public int GetUserId()
    {
        return _user.Id;
    }

    /// <inheritdoc />
    public async Task<UserDetailResponseDto> GetCallerUserDetailAsync()
    {
        _user ??= await _context.Users.Include(u => u.Roles).SingleAsync(u => u.Id == _userId);
        return new UserDetailResponseDto(_user);
    }

    /// <inheritdoc />
    public bool CanResetUserPassword(User targetUser)
    {
        return _user.Id != targetUser.Id &&
            _user.Role.Name == "Developer" &&
            targetUser.Role.Name != "Developer";
    }
}
