namespace NATS.Services.Validations.Validators;

public class SignInValidator : Validator<SignInRequestDto>
{
    public SignInValidator()
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