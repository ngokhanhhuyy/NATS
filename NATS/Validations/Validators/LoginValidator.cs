namespace NATS.Services.Validations.Validators;

public class LoginValidator : Validator<LoginRequestDto>
{
    public LoginValidator()
    {
        RuleFor(dto => dto.UserName)
            .NotNull()
            .NotEmpty()
            .WithName(dto => DisplayNames.Get(nameof(dto.UserName)));
        RuleFor(dto => dto.Password)
            .NotNull()
            .NotEmpty()
            .WithName(dto => DisplayNames.Get(nameof(dto.Password)));
    }
}