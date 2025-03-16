namespace NATS.Services;

/// <inheritdoc cref="ICatalogItemService" />
public class CatalogItemService
        :
            AbstractHasThumbnailService<CatalogItem, CatalogItemUpsertRequestDto>,
            ICatalogItemService
{
    public CatalogItemService(
            DatabaseContext context,
            IPhotoService photoService) : base(context, photoService)
    {
    }

    /// <inheritdoc />
    public async Task<List<CatalogItemBasicResponseDto>> GetListAsync(
            CatalogItemType? type = null)
    {
        IQueryable<CatalogItem> query = Context.CatalogItems.OrderBy(ci => ci.Id);

        if (type.HasValue)
        {
            query = query.Where(ci => ci.Type == type);
        }

        return await query.Select(ci => new CatalogItemBasicResponseDto(ci)).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<CatalogItemDetailResponseDto> GetDetailAsync(int id)
    {
        return await Context.CatalogItems
            .Include(bs => bs.Photos)
            .Select(bs => new CatalogItemDetailResponseDto(bs))
            .SingleOrDefaultAsync(bs => bs.Id == id)
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
            Photos = new List<CatalogItemPhoto>()
        };

        // Create photos.
        if (requestDto.Photos != null)
        {
            foreach (CatalogItemUpsertPhotoRequestDto photoRequestDto in requestDto.Photos)
            {
                CatalogItemPhoto photo = new CatalogItemPhoto
                {
                    Url = await PhotoService.CreateAsync(photoRequestDto.File, false)
                };

                catalogItem.Photos.Add(photo);
                PhotoUrlsToBeDeletedWhenFailure.Add(photo.Url);
            }
        }

        // Create new thumbnail if the request contains the data for it.
        if (requestDto.ThumbnailFile != null)
        {
            byte[] thumbnailFile = requestDto.ThumbnailFile;
            catalogItem.ThumbnailUrl = await PhotoService.CreateAsync(thumbnailFile, true);
            PhotoUrlsToBeDeletedWhenFailure.Add(catalogItem.ThumbnailUrl);
        }

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
            .Include(bs => bs.Photos)
            .SingleOrDefaultAsync(bs => bs.Id == id)
            ?? throw new ResourceNotFoundException();

        // Update the entity's properties.
        catalogItem.Name = requestDto.Name;
        catalogItem.Summary = requestDto.Summary;
        catalogItem.Detail = requestDto.Detail;
        
        // Update photos.
        if (requestDto.Photos != null)
        {
            for (int i = 0; i < requestDto.Photos.Count; i++)
            {
                CatalogItemUpsertPhotoRequestDto photoRequestDto = requestDto.Photos[i];
                CatalogItemPhoto photo;

                // Perform updating operation if this photo has id value.
                if (photoRequestDto.Id.HasValue)
                {   
                    photo = catalogItem.Photos
                        .SingleOrDefault(p => p.Id == photoRequestDto.Id);

                    // Ensure the photo entity with the given id exists.
                    if (photo == null)
                    {
                        string errorMessage = ErrorMessages.NotFound
                            .ReplacePropertyName(DisplayNames.Photo)
                            .ReplacePropertyName(DisplayNames.Id)
                            .ReplaceAttemptedValue(photoRequestDto.Id.ToString());
                        throw new OperationException($"photos[{i}]", errorMessage);
                    }

                    // Delete the old photo having URL stored in the entity property.
                    if (photoRequestDto.IsDeleted)
                    {
                        // Mark the url to be deleted later when the transaction succeeds.
                        PhotoUrlsToBeDeletedWhenSuccess.Add(photo.Url);
                        catalogItem.Photos.Remove(photo);
                        continue;
                    }
                }
                else if (!photoRequestDto.IsDeleted)
                {
                    // Create new photo if the request doesn't have id.
                    photo = new CatalogItemPhoto
                    {
                        Url = await PhotoService.CreateAsync(photoRequestDto.File, false),
                        ItemId = catalogItem.Id
                    };

                    Context.CatalogItemPhotos.Add(photo);

                    // Mark the created photo to be deleted later if the transaction fails.
                    PhotoUrlsToBeDeletedWhenFailure.Add(photo.Url);
                }
            }
        }

        // Save changes.
        await base.SaveUpdatedEntityAsync(catalogItem, requestDto);
        await transaction.CommitAsync();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        // Fetch the entity with the given id from the database and ensure it exists.
        CatalogItem catalogItem = await Context.CatalogItems
            .Include(bs => bs.Photos)
            .SingleOrDefaultAsync(bs => bs.Id == id)
            ?? throw new ResourceNotFoundException(
                nameof(CatalogItem),
                nameof(id),
                id.ToString());

        // Delete all photos.
        foreach (CatalogItemPhoto photo in catalogItem.Photos)
        {
            Context.CatalogItemPhotos.Remove(photo);
            PhotoUrlsToBeDeletedWhenSuccess.Add(photo.Url);
        }

        await base.SaveDeletedEntityAsync(catalogItem);
    }

    /// <inheritdoc />
    protected override sealed DbSet<CatalogItem> GetRepository(DatabaseContext context)
    {
        return context.CatalogItems;
    }
}