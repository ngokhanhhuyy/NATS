namespace NATS.Services;

/// <inheritdoc cref="IMemberService"/>
public class MemberService
    :
        AbstractHasThumbnailService<Member, MemberUpsertRequestDto>,
        IMemberService
{

    public MemberService(
            DatabaseContext context,
            IPhotoService photoService) : base(context, photoService)
    {
    }

    /// <inheritdoc/>
    public async Task<List<MemberResponseDto>> GetListAsync()
    {
        return await _context.Members
            .OrderBy(member => member.Id)
            .Select(member => new MemberResponseDto(member))
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<MemberResponseDto> GetSingleAsync(int id)
    {
        // Fetch the entity from the database and ensure it exists.
        return await _context.Members
            .Select(member => new MemberResponseDto(member))
            .SingleOrDefaultAsync(tm => tm.Id == id)
            ?? throw new ResourceNotFoundException(
                nameof(Member),
                nameof(id),
                id.ToString());
    }

    /// <inheritdoc/>
    protected override DbSet<Member> GetRepository(DatabaseContext context)
    {
        return context.Members;
    }

    /// <inheritdoc/>
    protected override async Task<Member> InitializeEntityAsync(
            MemberUpsertRequestDto requestDto)
    {
        Member member = await base.InitializeEntityAsync(requestDto);
        member.FullName = requestDto.FullName;
        member.RoleName = requestDto.RoleName;
        member.Description = requestDto.Description;

        return member;
    }

    /// <inheritdoc/>
    protected override async Task UpdateEntityAsync(
            Member member,
            MemberUpsertRequestDto requestDto)
    {
        await base.UpdateEntityAsync(member, requestDto);
        member.FullName = requestDto.FullName;
        member.RoleName = requestDto.RoleName;
        member.Description = requestDto.Description;
    }
}