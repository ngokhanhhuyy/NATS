namespace NATS.Services;

/// <inheritdoc/>
public class TeamMemberService : ITeamMemberService
{
    private readonly DatabaseContext _context;
    private readonly IPhotoService _photoService;

    public TeamMemberService(DatabaseContext context, IPhotoService photoService)
    {
        _context = context;
        _photoService = photoService;
    }

    /// <inheritdoc/>
    public async Task<List<TeamMemberResponseDto>> GetListAsync()
    {
        return await _context.TeamMembers
            .OrderBy(teamMember => teamMember.Id)
            .Select(teamMember => new TeamMemberResponseDto(teamMember))
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<TeamMemberResponseDto> GetSingleAsync(int id)
    {
        // Fetch the entity from the database and ensure it exists.
        return await _context.TeamMembers
            .Select(teamMember => new TeamMemberResponseDto(teamMember))
            .SingleOrDefaultAsync(tm => tm.Id == id)
            ?? throw new ResourceNotFoundException(
                nameof(TeamMember),
                nameof(id),
                id.ToString());
    }

    public async Task<int> CreateAsync(TeamMemberUpsertRequestDto upsertRequestDto)
    {
        // Initialize a new entity.
        TeamMember member = new TeamMember
        {
            FullName = upsertRequestDto.FullName,
            RoleName = upsertRequestDto.RoleName,
            Description = upsertRequestDto.Description,
            PhotoUrl = photoUrl
        };
        
        _context.TeamMembers.Add(member);
        // Save the photo if exist.
        string photoUrl = null;
        if (upsertRequestDto.PhotoFile != null)
        {
            ServiceResult<string> photoServiceResult;
            photoServiceResult = await _photoService.CreateAsync(
                upsertRequestDto.PhotoFile,
                "members",
                true);
            photoUrl = photoServiceResult.ResponseDto;
        }
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
            TeamMemberUpsertRequestDto upsertRequestDto)
    {
        // Validate data from request
        ValidationResult result = _validator.Validate(upsertRequestDto.TransformValues());
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
        if (upsertRequestDto.PhotoChanged)
        {
            ServiceResult<string> photoServiceResult;
            // Delete old photo if exists
            if (member.PhotoUrl != null)
            {
                photoServiceResult = _photoService.Delete(member.PhotoUrl);
                member.PhotoUrl = null;
            }
            // Create new photo if it's data is included in the request
            if (upsertRequestDto.PhotoFile != null)
            {
                photoServiceResult = await _photoService.CreateAsync(upsertRequestDto.PhotoFile, "members", true);
                member.PhotoUrl = photoServiceResult.ResponseDto;
            }
        }

        // Update team member entity's columns
        member.FullName = upsertRequestDto.FullName;
        member.RoleName = upsertRequestDto.RoleName;
        member.Description = upsertRequestDto.Description;

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