namespace NATS.Services.Validation.Validators;

public class HomePageSliderItemValidator : Validator<HomePageSliderItemRequestDto>
{
    public HomePageSliderItemValidator()
    {
        RuleFor(dto => dto.Title)
            .MaximumLength(100)
            .WithName(DisplayNames.Title);
        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.PhotoFile)
                .NotEmpty()
                .Must(IsValidImage)
                .WithMessage(ErrorMessages.Invalid)
                .WithName(DisplayNames.PhotoFile);
        });
        RuleSet("Update", () =>
        {
            RuleFor(dto => dto.PhotoFile)
                .NotEmpty()
                .Must(IsValidImage)
                .WithMessage(ErrorMessages.Invalid)
                .When(dto => dto.PhotoChanged)
                .WithName(DisplayNames.PhotoFile);
        });
    }
}