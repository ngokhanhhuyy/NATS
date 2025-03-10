namespace NATS.Services.Validations.Validators;

public class AboutUsIntroductionValidator : Validator<AboutUsIntroductionRequestDto>
{
    public AboutUsIntroductionValidator()
    {
        RuleFor(dto => dto.MainPhotoFile)
            .Must(IsValidImage)
            .WithMessage(ErrorMessages.Invalid)
            .When(dto => dto.MainPhotoFile != null)
            .WithName(DisplayNames.MainPhoto);
        RuleFor(dto => dto.MainQuoteContent)
            .NotEmpty()
            .MaximumLength(1000)
            .WithName(DisplayNames.MessageFromUs);
        RuleFor(dto => dto.AboutUsContent)
            .NotEmpty()
            .MaximumLength(1500)
            .WithName(DisplayNames.AboutUs);
        RuleFor(dto => dto.WhyChooseUsContent)
            .NotEmpty()
            .MaximumLength(1500)
            .WithName(DisplayNames.WhyChooseUs);
        RuleFor(dto => dto.OurDifferenceContent)
            .NotEmpty()
            .MaximumLength(1500)
            .WithName(DisplayNames.OurDifference);
        RuleFor(dto => dto.OurCultureContent)
            .NotEmpty()
            .MaximumLength(1500)
            .WithName(DisplayNames.OurCulture);
    }
}