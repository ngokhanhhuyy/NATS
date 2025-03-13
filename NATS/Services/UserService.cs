namespace NATS.Services;

/// <inheritdoc />
public class UserService : IUserService
{
	private readonly DatabaseContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IAuthorizationService _authorizationService;

	public UserService(
			DatabaseContext context,
			UserManager<User> userManager,
			IAuthorizationService authorizationService)
	{
		_context = context;
        _userManager = userManager;
        _authorizationService = authorizationService;
	}
	
	/// <inheritdoc />
	public async Task<UserListResponseDto> GetListAsync(UserListRequestDto requestDto)
	{
		// Initialize the response dto.
		UserListResponseDto responseDto = new UserListResponseDto();
		
		// Preparing the query.
		IQueryable<User> query = _context.Users
			.Include(u => u.Roles)
			.OrderBy(u => u.Id);

		int resultCount = await query.CountAsync();
		if (resultCount == 0)
		{
			responseDto.PageCount = 0;
			return responseDto;
		}

		responseDto.PageCount = 
			(int)Math.Ceiling((double)resultCount / requestDto.ResultsByPage);
		responseDto.Results = await query
			.Skip(requestDto.ResultsByPage * (requestDto.Page - 1))
			.Take(requestDto.ResultsByPage)
			.Select(u => new UserDetailResponseDto(u))
			.ToListAsync();

		return responseDto;
	}
	
	/// <inheritdoc />
	public async Task<int> GetCountAsync()
	{
		return await _context.Users.CountAsync();
	}
	
	/// <inheritdoc />
	public async Task<RoleResponseDto> GetRoleAsync(int id)
	{
        return await _context.Users
            .Include(u => u.Role)
            .Where(u => u.Id == id)
            .Select(u => new RoleResponseDto(u.Role))
			.SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException(nameof(User), nameof(id), id.ToString());
	}

    /// <inheritdoc />
    public async Task<UserDetailResponseDto> GetDetailAsync(int id)
    {
        User user = await _context.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Id == id)
            ?? throw new ResourceNotFoundException(nameof(User), nameof(id), id.ToString());

        return new UserDetailResponseDto(user);
    }

    /// <inheritdoc />
    public async Task ChangePasswordAsync(UserPasswordChangeRequestDto requestDto)
    {
        // Fetch the entity with given id and ensure the entity exists.
        int id = _authorizationService.GetUserId();
        User user = await _context.Users
            .SingleOrDefaultAsync(u => u.Id == id)
            ?? throw new ResourceNotFoundException(nameof(User), nameof(id), id.ToString());

        // Performing password change operation.
        IdentityResult result = await _userManager
            .ChangePasswordAsync(user, requestDto.CurrentPassword, requestDto.NewPassword);

        // Ensure the operation succeeded.
        if (!result.Succeeded)
        {
            throw new OperationException(
                nameof(requestDto.CurrentPassword),
                result.Errors.First().Description);
        }
    }

    /// <inheritdoc />
    public async Task ResetPasswordAsync(int id, UserPasswordResetRequestDto requestDto)
    {
        // Fetch the entity with given id and ensure the entity exists.
        User user = await _context.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Id == id)
            ?? throw new ResourceNotFoundException(nameof(User), nameof(id), id.ToString());

        // Performing password reset operation.
        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        IdentityResult result = await _userManager
            .ResetPasswordAsync(user, token, requestDto.NewPassword);

        // Ensure the operation succeeded.
        if (!result.Succeeded)
        {
            throw new OperationException(
                requestDto.NewPassword,
                result.Errors.First().Description);
        }
    }
}