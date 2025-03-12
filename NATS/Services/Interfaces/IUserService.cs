namespace NATS.Services.Interfaces;

public interface IUserService
{
    Task<ServiceResult<JwtResponseDto>> GetJwtAsync(SignInRequestDto requestDto);
    
    Task<ServiceResult<LoginResponseDto>> LoginAsync(SignInRequestDto requestDto);
    
    Task<ServiceResult<bool>> LogoutAsync();
    
    Task<ServiceResult<UserListResponseDto>> GetListAsync(UserListRequestDto requestDto);
    
    Task<ServiceResult<int>> GetCountAsync();
    
    Task SetCurrentUserId(int id);
    
    ServiceResult<UserBasicResponseDto> GetUserAsCurrentUser();
}
