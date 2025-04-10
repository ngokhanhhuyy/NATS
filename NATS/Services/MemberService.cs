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
        return await Context.Members
            .OrderBy(member => member.Id)
            .Select(member => new MemberResponseDto(member))
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<MemberResponseDto> GetSingleAsync(int id)
    {
        // Fetch the entity from the database and ensure it exists.
        return await Context.Members
            .Where(tm => tm.Id == id)
            .Select(member => new MemberResponseDto(member))
            .SingleOrDefaultAsync()
            ?? throw GetResourceNotFoundExceptionById(id);
    }

    /// <inheritdoc/>
    public async Task<int> CreateAsync(MemberUpsertRequestDto requestDto)
    {
        Member member = new Member
        {
            FullName = requestDto.FullName,
            RoleName = requestDto.RoleName,
            Description = requestDto.Description
        };

        return await base.SaveCreatedEntityAsync(member, requestDto);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(int id, MemberUpsertRequestDto requestDto)
    {
        // Fetch the entity from the database and ensure it exists.
        Member member = await Context.Members
            .SingleOrDefaultAsync(m => m.Id == id)
            ?? throw GetResourceNotFoundExceptionById(id);

        // Update the entity's properties.
        member.FullName = requestDto.FullName;
        member.RoleName = requestDto.RoleName;
        member.Description = requestDto.Description;

        // Save changes.
        await base.SaveUpdatedEntityAsync(member, requestDto);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int id)
    {
        // Fetch the entity from the database and ensure it exists.
        Member member = await Context.Members
            .SingleOrDefaultAsync(m => m.Id == id)
            ?? throw GetResourceNotFoundExceptionById(id);

        await base.SaveDeletedEntityAsync(member);
    }

    /// <inheritdoc/>
    protected override DbSet<Member> GetRepository(DatabaseContext context)
    {
        return context.Members;
    }
}