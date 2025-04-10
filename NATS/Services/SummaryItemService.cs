namespace NATS.Services;

/// <inheritdoc cref="ISummaryItemService" />
public class SummaryItemService
    :
        AbstractHasThumbnailService<SummaryItem, SummaryItemUpdateRequestDto>,
        ISummaryItemService
{

    public SummaryItemService(
            DatabaseContext context,
            IPhotoService photoService) : base(context, photoService)
    {
    }

    /// <inheritdoc />
    public async Task<List<SummaryItemResponseDto>> GetListAsync()
    {
        return await Context.SummaryItems
            .OrderBy(summaryItem => summaryItem.Id)
            .Take(4)
            .Select(summaryItem => new SummaryItemResponseDto(summaryItem))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<SummaryItemResponseDto> GetSingleAsync(int id)
    {
        return await Context.SummaryItems
            .Where(ii => ii.Id == id)
            .Select(summaryItem => new SummaryItemResponseDto(summaryItem))
            .SingleOrDefaultAsync()
            ?? throw GetResourceNotFoundExceptionById(id);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, SummaryItemUpdateRequestDto requestDto)
    {
        // Fetch the entity from the database and ensure it exists.
        SummaryItem item = await Context.SummaryItems
            .SingleOrDefaultAsync(ii => ii.Id == id)
            ?? throw GetResourceNotFoundExceptionById(id);

        // Update the entity's properties.
        item.Name = requestDto.Name;
        item.SummaryContent = requestDto.SummaryContent;
        item.DetailContent = requestDto.DetailContent;

        // Save changes.
        await base.SaveUpdatedEntityAsync(item, requestDto);
    }

    /// <inheritdoc />
    protected override sealed DbSet<SummaryItem> GetRepository(DatabaseContext context)
    {
        return context.SummaryItems;
    }
}