namespace NATS.Validation.Validators;

public class SliderItemUpsertValidator : Validator<SliderItemUpsertRequestDto>
{
    public SliderItemUpsertValidator()
    {
        RuleFor(dto => dto.Title)
            .MaximumLength(100)
            .WithName(DisplayNames.Title);
    }
}