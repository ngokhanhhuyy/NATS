namespace NATS.Services.Validations.Validators;

public class UserListValidator : Validator<UserListRequestDto>
{
    public UserListValidator() : base()
    {
        RuleFor(dto => dto.Page)
            .NotNull()
            .GreaterThanOrEqualTo(1)
            .WithName(dto => DisplayNames.Get(nameof(dto.Page)));
        RuleFor(dto => dto.ResultsByPage)
            .NotNull()
            .GreaterThanOrEqualTo(5)
            .LessThanOrEqualTo(50)
            .WithName(dto => DisplayNames.Get(nameof(dto.ResultsByPage)));
    }
}