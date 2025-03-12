namespace NATS.Models;

public class MemberUpsertModel
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

    [Display(Name = DisplayNames.Thumbnail)]
    public string ThumbnailUrl { get; set; }

    [Display(Name = DisplayNames.Thumbnail)]
    public IFormFile ThumbnailFile { get; set; }

    public bool ThumbnailChanged { get; set; } = false;

    [BindNever]
    public bool IsForCreating { get; set; } = true;

    public MemberUpsertModel() { }

    public MemberUpsertModel(MemberResponseDto responseDto)
    {
        Id = responseDto.Id;
        FullName = responseDto.FullName;
        RoleName = responseDto.RoleName;
        Description = responseDto.Description;
        ThumbnailUrl = responseDto.ThumbnailUrl;
        IsForCreating = false;
    }
}