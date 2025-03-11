namespace NATS.Services.Validation.Validators;

public class ContactUpsertValidator : Validator<ContactUpsertRequestDto>
{
    private const string phoneNumberRegex = @"^[^\-+\s][\d\-+\s]+$";
    private const string emailRegex = @"^\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b$";

    public ContactUpsertValidator()
    {
        RuleFor(dto => dto.Type)
            .IsInEnum()
            .WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.Type);

        RuleFor(dto => dto.Content)
            // Rules for phone and zalo numbers.
            .MaximumLength(20)
            .Matches(phoneNumberRegex)
            .WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.PhoneNumber)
            .When(dto =>
            {
                bool isPhoneNumber = dto.Type == ContactType.PhoneNumber;
                bool isZaloNumber = dto.Type == ContactType.ZaloNumber;
                return isPhoneNumber || isZaloNumber;
            })
            
            // Rules for email.
            .MaximumLength(255)
            .Matches(emailRegex)
            .WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.Email)
            .When(dto => dto.Type == ContactType.Email)

            // Rules for address.
            .MaximumLength(255)
            .WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.Address)
            .When(dto => dto.Type == ContactType.Address);


    }
}