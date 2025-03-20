namespace NATS.Validation.Validators;

public class GeneralSettingsUpdateValidator : Validator<GeneralSettingsUpdateRequestDto>
{
    public GeneralSettingsUpdateValidator()
    {
        RuleFor(dto => dto.ApplicationName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100)
            .WithName(dto => DisplayNames.Get(nameof(dto.ApplicationName)));
        RuleFor(dto => dto.ApplicationShortName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100)
            .WithName(dto => DisplayNames.Get(nameof(dto.ApplicationName)));
        RuleFor(dto => dto.FavIconFile)
            .Must(IsValidImage).WithMessage(ErrorMessages.Invalid)
            .When(dto => dto.FavIconFile != null)
            .WithName(dto => DisplayNames.Get(nameof(dto.ApplicationShortName)));
    }
}
