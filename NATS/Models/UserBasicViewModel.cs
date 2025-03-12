namespace NATS.Models;

public class UserDetailModel
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public RoleDetailModel Role { get; set; }

    public UserDetailModel(UserDetailResponseDto responseDto)
    {
        Id = responseDto.Id;
        UserName = responseDto.UserName;
        Role = new RoleDetailModel(responseDto.Role);
    }
}