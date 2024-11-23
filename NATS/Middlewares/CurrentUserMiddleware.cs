namespace NATS.Middlewares;

public class CurrentUserMiddleware
{
    private readonly RequestDelegate _next;

    public CurrentUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
            HttpContext context,
            IUserService userService,
            UserManager<User> userManager)
    {
        if (context.User.Identity!.IsAuthenticated)
        {
            // Parse the user id which is string in the cookie into integer.
            int userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Confirm if the user id exists in the database.
            bool userExists = await userManager.Users.AnyAsync(u => u.Id == userId);

            // Force signing out if the user id is invalid.
            if (!userExists)
            {
                await context.SignOutAsync(IdentityConstants.ApplicationScheme);
            }

            // Set the user id for further user data accessing through the request pipeline.
            await userService.SetCurrentUserId(userId);
        }
        await _next(context);
    }
}