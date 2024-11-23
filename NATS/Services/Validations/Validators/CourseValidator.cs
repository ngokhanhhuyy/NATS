namespace NATS.Services.Validations.Validators;

public class CourseValidator : Validator<CourseRequestDto>
{
    public CourseValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(50)
            .WithName(DisplayNames.Name);
        RuleFor(dto => dto.Summary)
            .MaximumLength(255)
            .WithName(DisplayNames.Summary);
        RuleFor(dto => dto.Detail)
            .MaximumLength(5000)
            .WithName(DisplayNames.Detail);
        RuleFor(dto => dto.ThumbnailFile)
            .Must(IsValidImage)
            .WithMessage(ErrorMessages.Invalid)
            .When(dto => dto.ThumbnailFile != null)
            .WithName(DisplayNames.ThumbnailFile);
        RuleSet("Create", () =>
        {
            RuleForEach(dto => dto.Sections)
                .Cascade(CascadeMode.Continue)
                .SetValidator(new CourseSectionValidator(), ruleSets: "Create");
            RuleForEach(dto => dto.Photos)
                .Cascade(CascadeMode.Continue)
                .SetValidator(new CoursePhotoValidator(), ruleSets: "Create");
        });
        RuleSet("Update", () =>
        {
            RuleForEach(dto => dto.Sections)
                .Cascade(CascadeMode.Continue)
                .SetValidator(new CourseSectionValidator(), ruleSets: "Update");
            RuleForEach(dto => dto.Photos)
                .Cascade(CascadeMode.Continue)
                .SetValidator(new CoursePhotoValidator(), ruleSets: "Update");
        });
    }
}
