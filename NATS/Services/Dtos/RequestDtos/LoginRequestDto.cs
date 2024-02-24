namespace NATS.Services.Dtos.RequestDtos;

public class LoginRequestDto : IRequestDto<LoginRequestDto>
{
    public string UserName { get; set; }
    public string Password { get; set; }

    public LoginRequestDto TransformValues()
    {
        UserName = UserName.ToNullIfEmpty();
        Password = Password.ToNullIfEmpty();
        return this;
    }
}