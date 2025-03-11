namespace NATS.Services.Validation.Validators;

public class CertificateUpsertValidator : Validator<CertificateUpsertRequestDto>
{
    public CertificateUpsertValidator()
    {
        RuleFor(dto => dto.Name)
            .MaximumLength(100)
            .WithName(DisplayNames.Name);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.ThumbnailFile)
                .NotNull()
                .Must(IsValidImage)
                .WithMessage(ErrorMessages.Invalid)
                .WithName(DisplayNames.PhotoFile);
        });

        RuleSet("Update", () =>
        {
            RuleFor(dto => dto.ThumbnailFile)
                .NotNull()
                .Must(IsValidImage)
                .WithMessage(ErrorMessages.Invalid)
                .When(dto => dto.ThumbnailChanged)
                .WithName(DisplayNames.PhotoFile);
        });
    }
}