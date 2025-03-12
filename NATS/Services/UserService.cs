namespace NATS.Services;

/// <inheritdoc />
public class UserService : IUserService
{
	private readonly DatabaseContext _context;
	
	public UserService(DatabaseContext context)
	{
		_context = context;
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
}