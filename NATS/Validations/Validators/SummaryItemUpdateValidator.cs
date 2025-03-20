namespace NATS.Services.Validations.Validators;

public class SummaryItemUpdateValidator : Validator<SummaryItemUpdateRequestDto>
{
    public SummaryItemUpdateValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(25)
            .WithName(DisplayNames.Name);
        RuleFor(dto => dto.SummaryContent)
            .NotEmpty()
            .MaximumLength(255)
            .WithName(DisplayNames.Summary);
        RuleFor(dto => dto.DetailContent)
            .NotEmpty()
            .MaximumLength(3000)
            .WithName(DisplayNames.Content);
        RuleFor(dto => dto.ThumbnailFile)
            .Must(IsValidImage)
            .When(dto => dto.ThumbnailFile != null && dto.ThumbnailChanged)
            .WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.ThumbnailFile);
    }
}
