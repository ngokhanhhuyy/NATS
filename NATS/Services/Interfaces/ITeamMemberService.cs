namespace NATS.Services.Interfaces;

public interface ITeamMembersService
{
    Task<ServiceResult<List<TeamMemberResponseDto>>> GetListAsync();

    Task<ServiceResult<TeamMemberResponseDto>> GetAsync(int id);

    Task<ServiceResult<TeamMemberResponseDto>> CreateAsync(TeamMemberRequestDto requestDto);

    Task<ServiceResult<TeamMemberResponseDto>> UpdateAsync(
        int id,
        TeamMemberRequestDto requestDto);

    Task<ServiceResult<int>> DeleteAsync(int id);
}