namespace NATS.Services;

public class TeamMembersService : ITeamMembersService
{
    private readonly DatabaseContext _context;
    private readonly IValidator<TeamMemberRequestDto> _validator;
    private readonly IPhotoService _photoService;

    public TeamMembersService(
            DatabaseContext context,
            IValidator<TeamMemberRequestDto> validator,
            IPhotoService photoService)
    {
        _context = context;
        _photoService = photoService;
        _validator = validator;
    }

    public async Task<ServiceResult<List<TeamMemberResponseDto>>> GetListAsync()
    {
        List<TeamMemberResponseDto> responseDtos = await _context.TeamMembers
            .OrderBy(tm => tm.Id)
            .Select(tm => new TeamMemberResponseDto
            {
                Id = tm.Id,
                FullName = tm.FullName,
                RoleName = tm.RoleName,
                Description = tm.Description,
                PhotoUrl = tm.PhotoUrl
            }).ToListAsync();
        return ServiceResult<List<TeamMemberResponseDto>>.Success(responseDtos);
    }

    public async Task<ServiceResult<TeamMemberResponseDto>> GetAsync(int id)
    {
        // Fetch for the entity
        TeamMember member = await _context.TeamMembers.SingleOrDefaultAsync(tm => tm.Id == id);
        if (member == null)
        {
            return ServiceResult<TeamMemberResponseDto>.Failed(
                ServiceError.NotFoundByProperty(
                    nameof(TeamMember),
                    nameof(id),
                    id.ToString()
                ));
        }

        // Return data
        TeamMemberResponseDto responseDto = new TeamMemberResponseDto
        {
            Id = member.Id,
            FullName = member.FullName,
            RoleName = member.RoleName,
            Description = member.Description,
            PhotoUrl = member.PhotoUrl
        };
        return ServiceResult<TeamMemberResponseDto>.Success(responseDto);
    }

    public async Task<ServiceResult<TeamMemberResponseDto>> CreateAsync(TeamMemberRequestDto requestDto)
    {
        // Validate data from request
        ValidationResult result = _validator.Validate(requestDto.TransformValues());
        if (!result.IsValid)
        {
            return ServiceResult<TeamMemberResponseDto>.Failed(result.Errors);
        }

        // Save the photo if exist
        string photoUrl = null;
        if (requestDto.PhotoFile != null)
        {
            ServiceResult<string> photoServiceResult;
            photoServiceResult = await _photoService.CreateAsync(
                requestDto.PhotoFile,
                "members",
                true);
            photoUrl = photoServiceResult.ResponseDto;
        }

        // Initialize team member entity
        TeamMember member = new TeamMember
        {
            FullName = requestDto.FullName,
            RoleName = requestDto.RoleName,
            Description = requestDto.Description,
            PhotoUrl = photoUrl
        };
        _context.TeamMembers.Add(member);
        await _context.SaveChangesAsync();

        return ServiceResult<TeamMemberResponseDto>.Success(new TeamMemberResponseDto
        {
            Id = member.Id,
            FullName = member.FullName,
            RoleName = member.RoleName,
            Description = member.Description,
            PhotoUrl = member.PhotoUrl
        });
    }

    public async Task<ServiceResult<TeamMemberResponseDto>> UpdateAsync(
            int id,
            TeamMemberRequestDto requestDto)
    {
        // Validate data from request
        ValidationResult result = _validator.Validate(requestDto.TransformValues());
        if (!result.IsValid)
        {
            return ServiceResult<TeamMemberResponseDto>.Failed(result.Errors);
        }

        // Fetch for the entity
        TeamMember member = await _context.TeamMembers.SingleOrDefaultAsync(tm => tm.Id == id);
        if (member == null)
        {
            return ServiceResult<TeamMemberResponseDto>.Failed(
                ServiceError.NotFoundByProperty(
                    nameof(TeamMember),
                    nameof(id),
                    id.ToString()
                ));
        }

        // Update photo
        if (requestDto.PhotoChanged)
        {
            ServiceResult<string> photoServiceResult;
            // Delete old photo if exists
            if (member.PhotoUrl != null)
            {
                photoServiceResult = _photoService.Delete(member.PhotoUrl);
                member.PhotoUrl = null;
            }
            // Create new photo if it's data is included in the request
            if (requestDto.PhotoFile != null)
            {
                photoServiceResult = await _photoService.CreateAsync(requestDto.PhotoFile, "members", true);
                member.PhotoUrl = photoServiceResult.ResponseDto;
            }
        }

        // Update team member entity's columns
        member.FullName = requestDto.FullName;
        member.RoleName = requestDto.RoleName;
        member.Description = requestDto.Description;

        // Save changes
        await _context.SaveChangesAsync();

        // Return data of the updated entity
        TeamMemberResponseDto responseDto = new TeamMemberResponseDto
        {
            Id = member.Id,
            FullName = member.FullName,
            RoleName = member.RoleName,
            Description = member.Description,
            PhotoUrl = member.PhotoUrl
        };
        return ServiceResult<TeamMemberResponseDto>.Success(responseDto);
    }

    public async Task<ServiceResult<int>> DeleteAsync(int id)
    {
        // Fetch the entity with given id from the database
        TeamMember teamMember = await _context.TeamMembers.SingleOrDefaultAsync(tm => tm.Id == id);

        // Ensure the entity exists in the database
        if (teamMember == null)
        {
            return ServiceResult<int>.Failed(ServiceError.NotFoundByProperty(
                nameof(TeamMember),
                nameof(id),
                id.ToString()
            ));
        }

        // Performing delete operation
        _context.TeamMembers.Remove(teamMember);

        // Save changes
        await _context.SaveChangesAsync();

        // Return the id of the deleted entity
        return ServiceResult<int>.Success(teamMember.Id);
    }
}