namespace NATS.Services.Validation.Validators;

public class BusinessCertificateValidator : Validator<BusinessCertificateRequestDto>
{
    public BusinessCertificateValidator()
    {
        RuleFor(dto => dto.Name)
            .MaximumLength(100)
            .WithName(DisplayNames.Name);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.PhotoFile)
                .NotNull()
                .Must(IsValidImage)
                .WithMessage(ErrorMessages.Invalid)
                .WithName(DisplayNames.PhotoFile);
        });
        RuleSet("Update", () =>
        {
            RuleFor(dto => dto.PhotoFile)
                .NotNull()
                .Must(IsValidImage)
                .WithMessage(ErrorMessages.Invalid)
                .When(dto => dto.PhotoChanged)
                .WithName(DisplayNames.PhotoFile);
        });
    }
}