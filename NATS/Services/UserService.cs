namespace HCManagement.Services;

public class UserService : IUserService
{
	private readonly DatabaseContext _context;
	private readonly UserManager<User> _userManager;
	private readonly RoleManager<Role> _roleManager;
	private readonly SignInManager<User> _signInManager;
	private readonly IValidator<LoginRequestDto> _loginValidator;
    private readonly IValidator<UserListRequestDto> _userListValidator;
	private int _currentUserId;
	private User _currentUser;
    private readonly IConfiguration _config;
	
	public UserService(
			DatabaseContext context,
			UserManager<User> userManager,
			RoleManager<Role> roleManager,
			SignInManager<User> signInManager,
			IValidator<LoginRequestDto> loginValidator,
            IValidator<UserListRequestDto> userListValidator,
			IConfiguration config)
	{
		_context = context;
		_userManager = userManager;
		_roleManager = roleManager;
		_signInManager = signInManager;
		_loginValidator = loginValidator;
        _userListValidator = userListValidator;
        _config = config;
	}

	public async Task<ServiceResult<JwtResponseDto>> GetJwtAsync(LoginRequestDto requestDto)
	{
		// Validate data from request
		ValidationResult result = _loginValidator.Validate(requestDto.TransformValues());
		if (!result.IsValid)
		{
			return ServiceResult<JwtResponseDto>.Failed(result.Errors);
		}

		// Check if user exists
		User user = _userManager.Users.SingleOrDefault(u => u.UserName == requestDto.UserName);
		if (user == null)
		{
			return ServiceResult<JwtResponseDto>.Failed(ServiceError.NotFoundByProperty(
				nameof(User),
				nameof(requestDto.UserName),
				requestDto.UserName
			));
		}

		// Check if password is correct
		bool passwordValid = await _userManager.CheckPasswordAsync(user, requestDto.Password);
		if (!passwordValid)
		{
			return ServiceResult<JwtResponseDto>.Failed(ServiceError.Mismatched(
				nameof(requestDto.Password)
			));
		}

		Microsoft.IdentityModel.Tokens.SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		Claim[] claims = 
		{
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Name, user.UserName!)
		};

		JwtSecurityToken token = new JwtSecurityToken(_config["Jwt:Issuer"],
			_config["Jwt:Issuer"],
			claims,
			expires: DateTime.Now.AddMinutes(120),
			signingCredentials: credentials);

		return ServiceResult<JwtResponseDto>.Success(new JwtResponseDto
		{
			JwtToken = new JwtSecurityTokenHandler().WriteToken(token)
		});
	}

	public async Task<ServiceResult<LoginResponseDto>> LoginAsync(LoginRequestDto requestDto)
	{
		// Validate data from request
		ValidationResult result = _loginValidator.Validate(requestDto.TransformValues());
		if (!result.IsValid)
		{
			return ServiceResult<LoginResponseDto>.Failed(result.Errors);
		}

		// Check if user exists
		int userId = await _userManager.Users
			.Where(u => u.UserName == requestDto.UserName)
			.Select(u => u.Id)
			.SingleOrDefaultAsync();
		if (userId == 0)
		{
			return ServiceResult<LoginResponseDto>.Failed(
				new ServiceError
				{
					PropertyName = nameof(requestDto.UserName),
					ErrorMessage = ErrorMessages.NotFound.Replace(
						"{EntityName}",
						DisplayNames.Get(nameof(requestDto.UserName)))
				}
			);
		}

		// Performing login
		Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager
			.PasswordSignInAsync(
				userName: requestDto.UserName,
				password: requestDto.Password,
				isPersistent: false,
				lockoutOnFailure: false);
		if (!signInResult.Succeeded)
		{
			return ServiceResult<LoginResponseDto>.Failed(
				ServiceError.Mismatched(nameof(requestDto.Password)));
		}

		return ServiceResult<LoginResponseDto>.Success(new LoginResponseDto
		{
			Id = userId
		});

	}

	public async Task<ServiceResult<bool>> LogoutAsync()
	{
		await _signInManager.SignOutAsync();
		return ServiceResult<bool>.Success(true);
	}
	
	public async Task<ServiceResult<UserListResponseDto>> GetListAsync(UserListRequestDto requestDto)
	{
		// Validate data from request
		ValidationResult result = _userListValidator.Validate(requestDto.TransformValues());
		if (!result.IsValid)
		{
			return ServiceResult<UserListResponseDto>.Failed(result.Errors);
		}

		UserListResponseDto responseDto = new UserListResponseDto();
		// Get result count
		IQueryable<User> query = _context.Users
			.Include(u => u.Roles)
			.OrderBy(u => u.Id);

		int resultCount = await query.CountAsync();
		if (resultCount == 0)
		{
			responseDto.PageCount = 0;
			return ServiceResult<UserListResponseDto>.Success(responseDto);
		}
		responseDto.PageCount = (int)Math.Ceiling((double)resultCount / requestDto.ResultsByPage);
		responseDto.Results = await query
			.Skip(requestDto.ResultsByPage * (requestDto.Page - 1))
			.Take(requestDto.ResultsByPage)
			.Select(u => new UserBasicResponseDto
			{
				Id = u.Id,
				UserName = u.UserName,
			}).ToListAsync();
		return ServiceResult<UserListResponseDto>.Success(responseDto);
	}

	public async Task SetCurrentUserId(int id)
	{
		User user = await _userManager.Users
			.Include(u => u.Roles)
			.SingleOrDefaultAsync(u => u.Id == id);
		_currentUser = user ?? throw new InvalidOperationException($"User with id '{id}' cannot be found.");
		_currentUserId = id;
	}
	
	public ServiceResult<UserBasicResponseDto> GetUserAsCurrentUser()
	{
		UserBasicResponseDto responseDto = new UserBasicResponseDto
		{
			Id = _currentUser.Id,
			UserName = _currentUser.UserName,
			Role = new RoleResponseDto
			{
				Id = _currentUser.Role.Id,
				Name = _currentUser.Role.Name,
				DisplayName = _currentUser.Role.DisplayName
			}
		};
		return ServiceResult<UserBasicResponseDto>.Success(responseDto);
	}
}