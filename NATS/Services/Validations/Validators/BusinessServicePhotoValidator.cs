namespace NATS.Services.Validations.Validators;

public class BusinessServicePhotoValidator : Validator<BusinessServicePhotoRequestDto>
{
    public BusinessServicePhotoValidator()
    {
        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.File)
                .NotEmpty()
                .Must(IsValidImage)
                .WithMessage(ErrorMessages.Invalid)
                .WithName(DisplayNames.PhotoFile);
        });
        RuleSet("Update", () =>
        {
            RuleFor(dto => dto.File)
                .Must(IsValidImage)
                .WithMessage(ErrorMessages.Invalid)
                .When(dto => dto.File != null)
                .WithName(DisplayNames.PhotoFile);
        });
    }
}
