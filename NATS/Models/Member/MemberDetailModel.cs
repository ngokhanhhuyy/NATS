namespace NATS.Models;

public class MemberDetailModel
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }
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