namespace NATS.Validation.Validators;

public class CertificateUpsertValidator : Validator<CertificateUpsertRequestDto>
{
    public CertificateUpsertValidator()
    {
        RuleFor(dto => dto.Name)
            .MaximumLength(100)
            .WithName(DisplayNames.Name);
    }
}