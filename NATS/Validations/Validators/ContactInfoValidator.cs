namespace NATS.Services.Validation.Validators;

public class ContactInfoValidator : Validator<ContactInfoRequestDto>
{
    private const string phoneNumberRegex = @"^[^\-+\s][\d\-+\s]+$";
    private const string emailRegex = @"^\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b$";

    public ContactInfoValidator()
    {
        RuleFor(dto => dto.PhoneNumber)
            .MaximumLength(20)
            .Matches(phoneNumberRegex)
            .WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.PhoneNumber);
        RuleFor(dto => dto.ZaloNumber)
            .MaximumLength(20)
            .Matches(phoneNumberRegex)
            .WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.Zalo);
        RuleFor(dto => dto.Email)
            .MaximumLength(255)
            .Matches(emailRegex)
            .WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.Email);
        RuleFor(dto => dto.Address)
            .MaximumLength(255)
            .WithName(DisplayNames.Address);
    }
}