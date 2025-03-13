namespace NATS.Services.Dtos.RequestDtos;

public class UserListRequestDto : IRequestDto
{
	public int Page { get; set; }
    public int ResultsByPage { get; set; } = 15;

    public void TransformValues()
    {
    }
}