namespace NATS.Services.Dtos.RequestDtos;

public class UserCreateRequestDto : IRequestDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ConfirmationPassword { get; set; }
    public string RoleName { get; set; }

    public void TransformValues()
    {
        UserName = UserName.ToNullIfEmpty();
        Password = Password.ToNullIfEmpty();
        ConfirmationPassword = ConfirmationPassword.ToNullIfEmpty();
        RoleName = RoleName.ToNullIfEmpty();
    }
}
