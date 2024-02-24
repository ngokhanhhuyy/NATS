namespace NATS.Services.Dtos.ResponseDtos;

public class UserBasicResponseDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public RoleResponseDto Role { get; set; }
}