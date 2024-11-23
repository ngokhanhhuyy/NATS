namespace NATS.Models;

public class CourseDetailViewModel
{
    [Display(Name = DisplayNames.Id)]
    public int Id { get; set; }

    [Display(Name = DisplayNames.Name)]
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Display(Name = DisplayNames.Summary)]
    [MaxLength(255)]
    public string Summary { get; set; }

    [Display(Name = DisplayNames.Detail)]
    [MaxLength(5000)]
    public string Detail { get; set; }

    [Display(Name = DisplayNames.Thumbnail)]
    public string ThumbnailUrl { get; set; }

    [Display(Name = DisplayNames.ThumbnailFile)]
    public IFormFile ThumbnailFile { get; set; }

    public bool ThumbnailChanged { get; set; }

    [Display(Name = DisplayNames.CourseSection)]
    public List<CourseSectionViewModel> Sections { get; set; }

    [Display(Name = DisplayNames.Photo)]
    public List<CoursePhotoViewModel> Photos { get; set; }

    [BindNever]
    public bool IsForCreating { get; set; } = false;

    [BindNever]
    public bool WasValidated { get; set; } = false;
}
