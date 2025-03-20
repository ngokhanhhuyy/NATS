namespace NATS.Validation.Validators;

public class CatalogItemUpsertValidator : Validator<CatalogItemUpsertRequestDto>
{
    public CatalogItemUpsertValidator()
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
            RuleForEach(dto => dto.Photos)
                .Cascade(CascadeMode.Continue)
                .SetValidator(new CatalogItemUpsertPhotoValidator(), ruleSets: "Create");
        });

        RuleSet("Update", () =>
        {
            RuleForEach(dto => dto.Photos)
                .Cascade(CascadeMode.Continue)
                .SetValidator(new CatalogItemUpsertPhotoValidator(), ruleSets: "Update");
        });
    }
}
