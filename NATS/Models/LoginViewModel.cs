namespace NATS.Models;

public class LoginViewModel
{
    [Display(Name = DisplayNames.UserName)]
    public string UserName { get; set; }

    [Display(Name = DisplayNames.Password)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool WasValidated { get; set; }
}