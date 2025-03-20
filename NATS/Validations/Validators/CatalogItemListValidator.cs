namespace NATS.Validation.Validators;

public class CatalogItemListValidator : Validator<CatalogItemListRequestDto>
{
    public CatalogItemListValidator()
    {
        RuleFor(dto => dto.Type)
            .IsInEnum()
            .WithName(DisplayNames.Type);
    }
}