namespace NATS.Models;

public class CourseSectionViewModel
{
    [Display(Name = DisplayNames.Id)]
    public int? Id { get; set; }

    [Display(Name = DisplayNames.Content)]
    [Required]
    [MaxLength(200)]
    public string Content { get; set; }

    public bool IsDeleted { get; set; }
}
