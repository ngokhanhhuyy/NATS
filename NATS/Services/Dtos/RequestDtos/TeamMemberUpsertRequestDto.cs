namespace NATS.Services.Dtos.RequestDtos;

public class TeamMemberUpsertRequestDto : IRequestDto<TeamMemberUpsertRequestDto>
{
    public byte[] PhotoFile { get; set; }
    public string FullName { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }
    public bool PhotoChanged { get; set; } = false;

    public TeamMemberUpsertRequestDto TransformValues()
    {
        FullName = FullName.ToNullIfEmpty();
        RoleName = RoleName.ToNullIfEmpty();
        Description = Description.ToNullIfEmpty();
        return this;
    }
}