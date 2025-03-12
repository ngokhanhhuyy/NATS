namespace NATS.Services.Dtos.ResponseDtos;

public class MemberResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }
    public string ThumbnailUrl { get; set; }

    public MemberResponseDto(Member teamMember)
    {
        Id = teamMember.Id;
        FullName = teamMember.FullName;
        RoleName = teamMember.RoleName;
        Description = teamMember.Description;
        ThumbnailUrl = teamMember.ThumbnailUrl;
    }
}