namespace NATS.Validation.Validators;

public class MemberUpsertValidator : Validator<MemberUpsertRequestDto>
{
    public MemberUpsertValidator()
    {
        RuleFor(dto => dto.FullName)
            .NotEmpty()
            .MaximumLength(50)
            .WithName(DisplayNames.FullName);
        RuleFor(dto => dto.RoleName)
            .NotEmpty()
            .MaximumLength(50)
            .WithName(DisplayNames.RoleName);
        RuleFor(dto => dto.Description)
            .NotEmpty()
            .MaximumLength(400)
            .WithName(DisplayNames.Description);
    }
}