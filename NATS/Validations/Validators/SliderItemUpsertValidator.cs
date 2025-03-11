namespace NATS.Services.Validation.Validators;

public class SliderItemUpsertValidator : Validator<SliderItemUpsertRequestDto>
{
    public SliderItemUpsertValidator()
    {
        RuleFor(dto => dto.Title)
            .MaximumLength(100)
            .WithName(DisplayNames.Title);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.ThumbnailFile)
                .NotEmpty()
                .Must(IsValidImage)
                .WithMessage(ErrorMessages.Invalid)
                .WithName(DisplayNames.PhotoFile);
        });

        RuleSet("Update", () =>
        {
            RuleFor(dto => dto.ThumbnailFile)
                .NotEmpty()
                .Must(IsValidImage)
                .WithMessage(ErrorMessages.Invalid)
                .When(dto => dto.ThumbnailChanged)
                .WithName(DisplayNames.PhotoFile);
        });
    }
}