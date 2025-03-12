namespace NATS.Services.Dtos.ResponseDtos;

public class UserDetailResponseDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public RoleResponseDto Role { get; set; }

    public UserDetailResponseDto(User user)
    {
        Id = user.Id;
        UserName = user.UserName;
        Role = new RoleResponseDto(user.Role);
    }
}