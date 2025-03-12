namespace NATS.Services;

/// <inheritdoc/>
public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthenticationService(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    /// <inheritdoc />
    public async Task<int> SignInAsync(SignInRequestDto requestDto)
    {
        // Check if user exists.
        User user = await _userManager.Users
            .Include(u => u.Roles)
            .AsSplitQuery()
            .SingleOrDefaultAsync(u => u.UserName == requestDto.UserName);

        string errorMessage;
        if (user == null)
        {
            errorMessage = ErrorMessages.NotFoundByProperty
                .ReplaceResourceName(DisplayNames.User)
                .ReplacePropertyName(DisplayNames.UserName)
                .ReplaceAttemptedValue(requestDto.UserName);
            throw new OperationException(nameof(requestDto.UserName), errorMessage);
        }

        // Check the password.
        Microsoft.AspNetCore.Identity.SignInResult signInResult;
        signInResult = await _signInManager
            .CheckPasswordSignInAsync(user, requestDto.Password, lockoutOnFailure: false);

        if (!signInResult.Succeeded)
        {
            errorMessage = ErrorMessages.Incorrect.ReplacePropertyName(DisplayNames.Password);
            throw new OperationException(nameof(requestDto.Password), errorMessage);
        }

        // Prepare the claims to be added in to the generating cookie.
        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.Name)
        ];

        // Perform sign in operation.
        await _signInManager.SignInWithClaimsAsync(user, false, claims);

        return user.Id;
    }

    /// <inheritdoc />
    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}