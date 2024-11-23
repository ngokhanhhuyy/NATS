namespace NATS.Services.Validations.Validators;

public class BusinessServiceValidator : Validator<BusinessServiceRequestDto>
{
    public BusinessServiceValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(50)
            .WithName(DisplayNames.Name);
        RuleFor(dto => dto.Summary)
            .MaximumLength(255)
            .WithName(DisplayNames.Summary);
        RuleFor(dto => dto.Detail)
            .MaximumLength(5000)
            .WithName(DisplayNames.Detail);
        RuleFor(dto => dto.ThumbnailFile)
            .Must(IsValidImage)
            .WithMessage(ErrorMessages.Invalid)
            .When(dto => dto.ThumbnailFile != null)
            .WithName(DisplayNames.ThumbnailFile);
        RuleSet("Create", () =>
        {
            RuleForEach(dto => dto.Features)
                .Cascade(CascadeMode.Continue)
                .SetValidator(new BusinessServiceFeatureValidator(), ruleSets: "Create");
            RuleForEach(dto => dto.Photos)
                .Cascade(CascadeMode.Continue)
                .SetValidator(new BusinessServicePhotoValidator(), ruleSets: "Create");
        });
        RuleSet("Update", () =>
        {
            RuleForEach(dto => dto.Features)
                .Cascade(CascadeMode.Continue)
                .SetValidator(new BusinessServiceFeatureValidator(), ruleSets: "Update");
            RuleForEach(dto => dto.Photos)
                .Cascade(CascadeMode.Continue)
                .SetValidator(new BusinessServicePhotoValidator(), ruleSets: "Update");
        });
    }
}
