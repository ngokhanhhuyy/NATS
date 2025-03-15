namespace NATS.Models;

public class SignInModel
{
    public string UserName { get; set; }
    public string Password { get; set; }

    public SignInRequestDto ToRequestDto()
    {
        SignInRequestDto requestDto = new SignInRequestDto
        {
            UserName = UserName,
            Password = Password
        };

        requestDto.TransformValues();

        return requestDto;
    }
}