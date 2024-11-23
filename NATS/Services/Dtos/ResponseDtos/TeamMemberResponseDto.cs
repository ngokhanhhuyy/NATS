namespace NATS.Services.Dtos.ResponseDtos;

public class TeamMemberResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }
    public string PhotoUrl { get; set; }
}