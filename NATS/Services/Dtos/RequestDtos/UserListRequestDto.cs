namespace NATS.Services.Dtos.RequestDtos;

public class UserListRequestDto : IRequestDto<UserListRequestDto>
{
	public int Page { get; set; }
    public int ResultsByPage { get; set; } = 15;

    public UserListRequestDto TransformValues()
    {
        return this;
    }
}