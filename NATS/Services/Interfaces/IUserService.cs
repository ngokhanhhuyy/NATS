namespace NATS.Services.Interfaces;

public interface IUserService
{
    Task<ServiceResult<LoginResponseDto>> LoginAsync(LoginRequestDto requestDto);
    Task<ServiceResult<bool>> LogoutAsync();
    Task SetCurrentUserId(int id);
    ServiceResult<UserBasicResponseDto> GetUserAsCurrentUser();
}
