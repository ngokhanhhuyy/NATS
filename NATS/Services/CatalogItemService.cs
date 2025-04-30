namespace NATS.Services;

/// <inheritdoc cref="ICatalogItemService" />
public class CatalogItemService
        :
            AbstractUpsertableService<CatalogItem, CatalogItemUpsertRequestDto>,
            ICatalogItemService
{
    private readonly IDbContextFactory<DatabaseContext> _contextFactory;

    public CatalogItemService(
            DatabaseContext context,
            IDbContextFactory<DatabaseContext> contextFactory) : base(context)
    {
        _contextFactory = contextFactory;
    }

    /// <inheritdoc />
    public async Task<List<CatalogItemBasicResponseDto>> GetListAsync(
            CatalogItemListRequestDto requestDto)
    {
        IQueryable<CatalogItem> query = Context.CatalogItems.OrderBy(ci => ci.Id);

        if (requestDto.Type.HasValue)
        {
            query = query.Where(ci => ci.Type == requestDto.Type);
        }

        return await query.Select(ci => new CatalogItemBasicResponseDto(ci)).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<CatalogItemDetailResponseDto> GetDetailAsync(int id)
    {
        return await Context.CatalogItems
            .Where(ci => ci.Id == id)
            .Select(ci => new CatalogItemDetailResponseDto(
                ci,
                Context.CatalogItems
                    .Where(oci => oci.Id != id && oci.Type == ci.Type)
                    .ToList()))
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException();
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(CatalogItemUpsertRequestDto requestDto)
    {
        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await Context.Database
            .BeginTransactionAsync();

        // Initialize the entity.
        CatalogItem catalogItem = new CatalogItem
        {
            Name = requestDto.Name,
            Summary = requestDto.Summary,
            Detail = requestDto.Detail,
            ThumbnailUrl = requestDto.ThumbnailUrl
        };

        // Save changes.
        int createdId = await base.SaveCreatedEntityAsync(catalogItem, requestDto);
        await transaction.CommitAsync();

        return createdId;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, CatalogItemUpsertRequestDto requestDto)
    {
        // Use transaction for atomic operations.
        await using IDbContextTransaction transaction = await Context.Database
            .BeginTransactionAsync();

        // Fetch the entity in the database.
        CatalogItem catalogItem = await Context.CatalogItems
            .SingleOrDefaultAsync(bs => bs.Id == id)
            ?? throw new ResourceNotFoundException();

        // Update the entity's properties.
        catalogItem.Name = requestDto.Name;
        catalogItem.Summary = requestDto.Summary;
        catalogItem.Detail = requestDto.Detail;
        catalogItem.ThumbnailUrl = requestDto.ThumbnailUrl;

        // Save changes.
        await base.SaveUpdatedEntityAsync(catalogItem, requestDto);
        await transaction.CommitAsync();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        // Fetch the entity with the given id from the database and ensure it exists.
        CatalogItem catalogItem = await Context.CatalogItems
            .SingleOrDefaultAsync(bs => bs.Id == id)
            ?? throw new ResourceNotFoundException(
                nameof(CatalogItem),
                nameof(id),
                id.ToString());

        await base.SaveDeletedEntityAsync(catalogItem);
    }

    /// <inheritdoc />
    protected override sealed DbSet<CatalogItem> GetRepository(DatabaseContext context)
    {
        return context.CatalogItems;
    }
}