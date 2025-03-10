namespace NATS.Services.Dtos.RequestDtos;

public class TeamMemberRequestDto : IRequestDto<TeamMemberRequestDto>
{
    public byte[] PhotoFile { get; set; }
    public string FullName { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }

    public bool PhotoChanged { get; set; } = false;

    public TeamMemberRequestDto TransformValues()
    {
        FullName = FullName.ToNullIfEmpty();
        RoleName = RoleName.ToNullIfEmpty();
        Description = Description.ToNullIfEmpty();
        return this;
    }
}