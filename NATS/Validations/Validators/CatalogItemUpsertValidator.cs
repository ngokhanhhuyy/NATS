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
    }
}
