namespace NATS.Models;

public class SignInModel
{
    [Display(Name = DisplayNames.UserName)]
    [MaxLength(255)]
    public string UserName { get; set; }

    [Display(Name = DisplayNames.Password)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool WasValidated { get; set; }

    public SignInRequestDto ToRequestDto()
    {
        SignInRequestDto requestDto = new SignInRequestDto
        {
            UserName = UserName,
            Password = Password
        };

        return requestDto.TransformValues();
    }
}