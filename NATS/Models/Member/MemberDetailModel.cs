namespace NATS.Models;

public class MemberDetailModel
{
    [Display(Name = DisplayNames.Id)]
    public int Id { get; set; }

    [Display(Name = DisplayNames.FullName)]
    public string FullName { get; set; }

    [Display(Name = DisplayNames.RoleName)]
    public string RoleName { get; set; }

    [Display(Name = DisplayNames.Description)]
    public string Description { get; set; }

    [Display(Name = DisplayNames.Thumbnail)]
    public string ThumbnailUrl { get; set; }

    public MemberDetailModel(MemberResponseDto responseDto)
    {
        Id = responseDto.Id;
        FullName = responseDto.FullName;
        RoleName = responseDto.RoleName;
        Description = responseDto.Description;
        ThumbnailUrl = responseDto.ThumbnailUrl;
    }
}