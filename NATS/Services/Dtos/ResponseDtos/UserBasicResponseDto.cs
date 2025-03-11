namespace NATS.Services.Dtos.ResponseDtos;

public class UserBasicResponseDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public RoleResponseDto Role { get; set; }

    public UserBasicResponseDto(User user)
    {
        Id = user.Id;
        UserName = user.UserName;
        Role = new RoleResponseDto(user.Role);
    }
}