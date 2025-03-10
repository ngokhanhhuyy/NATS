namespace NATS.Services.Validations.Validators;

public class ProductValidator : Validator<ProductRequestDto>
{
    public ProductValidator()
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
                .SetValidator(new ProductFeatureValidator(), ruleSets: "Create");
            RuleForEach(dto => dto.Photos)
                .Cascade(CascadeMode.Continue)
                .SetValidator(new ProductPhotoValidator(), ruleSets: "Create");
        });
        RuleSet("Update", () =>
        {
            RuleForEach(dto => dto.Features)
                .Cascade(CascadeMode.Continue)
                .SetValidator(new ProductFeatureValidator(), ruleSets: "Update");
            RuleForEach(dto => dto.Photos)
                .Cascade(CascadeMode.Continue)
                .SetValidator(new ProductPhotoValidator(), ruleSets: "Update");
        });
    }
}
