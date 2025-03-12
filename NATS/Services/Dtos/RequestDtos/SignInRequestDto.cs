namespace NATS.Services.Dtos.RequestDtos;

public class SignInRequestDto : IRequestDto<SignInRequestDto>
{
    public string UserName { get; set; }
    public string Password { get; set; }

    public SignInRequestDto TransformValues()
    {
        UserName = UserName.ToNullIfEmpty();
        Password = Password.ToNullIfEmpty();
        return this;
    }
}