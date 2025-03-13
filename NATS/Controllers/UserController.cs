namespace NATS.Controllers.Api;

[Route("/Api/User")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IValidator<UserListRequestDto> _listValidator;
    private readonly IValidator<UserPasswordChangeRequestDto> _passwordChangeValidator;
    private readonly IValidator<UserPasswordResetRequestDto> _passwordResetValidator;

    public UserController(
            IUserService userService,
            IAuthorizationService authorizationService,
            IValidator<UserListRequestDto> listValidator,
            IValidator<UserPasswordChangeRequestDto> passwordChangeValidator,
            IValidator<UserPasswordResetRequestDto> passwordResetValidator)
    {
        _userService = userService;
        _authorizationService = authorizationService;
        _listValidator = listValidator;
        _passwordChangeValidator = passwordChangeValidator;
        _passwordResetValidator = passwordResetValidator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UserList([FromQuery] UserListRequestDto requestDto)
    {
        // Validate data from request.
        requestDto.TransformValues();
        ValidationResult validationResult;
        validationResult = _listValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        // Perform fetching operation.
        UserListResponseDto responseDto = await _userService.GetListAsync(requestDto);
        return Ok(responseDto);
    }

    [HttpGet("{id:int}/Role")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UserRole(int id)
    {
        try
        {
            return Ok(await _userService.GetRoleAsync(id));
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(exception);
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UserDetail(int id)
    {
        try
        {
            UserDetailResponseDto responseDto;
            responseDto = await _userService.GetDetailAsync(id);
            return Ok(responseDto);
        }
        catch (ResourceNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("Caller")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CallerDetail()
    {
        int callerId = _authorizationService.GetUserId();
        UserDetailResponseDto responseDto = await _userService.GetDetailAsync(callerId);
        return Ok(responseDto);
    }

    [HttpPut("ChangePassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ChangeUserPassword(
            [FromBody] UserPasswordChangeRequestDto requestDto)
    {
        // Validate data from the request.
        requestDto.TransformValues();
        ValidationResult validationResult;
        validationResult = _passwordChangeValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        // Perform the password change operation.
        try
        {
            await _userService.ChangePasswordAsync(requestDto);
            return Ok();
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
        catch (AuthorizationException)
        {
            return Forbid();
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return UnprocessableEntity(ModelState);
        }
    }

    [HttpPut("{id:int}/ResetPassword")]
    [Authorize(Roles = "Developer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ResetUserPassword(
            int id,
            [FromBody] UserPasswordResetRequestDto requestDto)
    {
        // Validate data from the request.
        requestDto.TransformValues();
        ValidationResult validationResult;
        validationResult = _passwordResetValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        // Perform the password reset operation.
        try
        {
            await _userService.ResetPasswordAsync(id, requestDto);
            return Ok();
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
        catch (AuthorizationException)
        {
            return Forbid();
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return UnprocessableEntity(exception);
        }
    }

    [HttpGet("{id:int}/PasswordResetPermission")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPasswordResetPermission(int id)
    {
        int callerUserId = _authorizationService.GetUserId();
        RoleResponseDto callerUserRole = await _userService.GetRoleAsync(callerUserId);
        RoleResponseDto targetUserRole = await _userService.GetRoleAsync(id);
        return Ok(callerUserRole.Name == "Developer" && targetUserRole.Name != "Developer");
    }

    [HttpGet("CreatingPermission")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCreatingPermission()
    {
        int callerUserId = _authorizationService.GetUserId();
        RoleResponseDto callerUserRole = await _userService.GetRoleAsync(callerUserId);
        return Ok(callerUserRole.Name == "Developer");
    }
}