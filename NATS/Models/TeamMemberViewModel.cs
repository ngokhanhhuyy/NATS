namespace NATS.Models;

public class TeamMemberViewModel
{
    [Display(Name = DisplayNames.Id)]
    public int Id { get; set; }

    [Display(Name = DisplayNames.FullName)]
    [Required]
    [MaxLength(50)]
    public string FullName { get; set; }

    [Display(Name = DisplayNames.RoleName)]
    [Required]
    [MaxLength(50)]
    public string RoleName { get; set; }

    [Display(Name = DisplayNames.Description)]
    [Required]
    [MaxLength(400)]
    public string Description { get; set; }

    [Display(Name = DisplayNames.Photo)]
    public string PhotoUrl { get; set; }

    [Display(Name = DisplayNames.Photo)]
    public IFormFile PhotoFile { get; set; }

    public bool PhotoChanged { get; set; } = false;

    [BindNever]
    public bool IsForCreating { get; set; } = false;
}