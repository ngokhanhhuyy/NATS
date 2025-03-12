namespace NATS.Services.Dtos.ResponseDtos;

public class UserListResponseDto
{
    public int PageCount { get; set; }
    public ICollection<UserDetailResponseDto> Results { get; set; }
}