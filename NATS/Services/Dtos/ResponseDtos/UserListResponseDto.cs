namespace NATS.Services.Dtos.ResponseDtos;

public class UserListResponseDto
{
    public int PageCount { get; set; }
    public ICollection<UserBasicResponseDto> Results { get; set; }
}