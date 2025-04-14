namespace NATS.Protected.Models;

public class SignInViewModel
{
    [Display(Name = DisplayNames.UserName)]
    [MaxLength(50)]
    public string UserName { get; set; }

    [Display(Name = DisplayNames.Password)]
    [MaxLength(50)]

    public string Password { get; set; }

    [BindNever]
    public GeneralSettingsDetailModel GeneralSettings { get; set; }

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