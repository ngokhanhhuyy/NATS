namespace NATS.Services.Validations.Validators;

public class ProductFeatureValidator : Validator<ProductFeatureRequestDto>
{
    public ProductFeatureValidator()
    {
        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.Content)
                .NotEmpty()
                .MaximumLength(200)
                .WithName(DisplayNames.Content);
        });
        RuleSet("Update", () =>
        {
            RuleFor(dto => dto.Content)
                .NotEmpty()
                .MaximumLength(200)
                .When(dto => !dto.IsDeleted)
                .WithName(DisplayNames.Content);
        });
    }
}
