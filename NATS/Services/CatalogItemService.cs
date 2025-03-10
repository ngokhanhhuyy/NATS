using NATS.Services.Exceptions;

namespace NATS.Services;

public class CatalogItemService : ICatalogItemService
{
    private readonly DatabaseContext _context;
    private readonly IValidator<CatalogItemUpsertRequestDto> _validator;
    private readonly IPhotoService _photoService;

    public CatalogItemService(
            DatabaseContext context,
            IValidator<CatalogItemUpsertRequestDto> validator,
            IPhotoService photoService)
    {
        _context = context;
        _validator = validator;
        _photoService = photoService;
    }

    /// <inheritdoc />
    public async Task<List<CatalogItemBasicResponseDto>> GetListAsync(CatalogItemType type)
    {
        return await _context.CatalogItems
            .OrderBy(ci => ci.Id)
            .Where(ci => ci.Type == type)
            .Select(ci => new CatalogItemBasicResponseDto(ci))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<CatalogItemDetailResponseDto> GetDetailAsync(int id)
    {
        return await _context.CatalogItems
            .Include(bs => bs.Photos)
            .Select(bs => new CatalogItemDetailResponseDto(bs))
            .SingleOrDefaultAsync(bs => bs.Id == id)
            ?? throw new ResourceNotFoundException();
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(CatalogItemUpsertRequestDto requestDto)
    {
        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Initialize the entity.
        CatalogItem catalogItem = new CatalogItem
        {
            Name = requestDto.Name,
            Summary = requestDto.Summary,
            Detail = requestDto.Detail,
            Photos = new List<CatalogItemPhoto>()
        };

        _context.CatalogItems.Add(catalogItem);

        // Create new thumbnail if the request contains the data for it.
        if (requestDto.ThumbnailFile != null)
        {
            catalogItem.ThumbnailUrl = await _photoService.CreateAsync(
                requestDto.ThumbnailFile,
                "catalogItems",
                true);
        }

        // Create photos.
        if (requestDto.Photos != null)
        {
            foreach (CatalogItemUpsertPhotoRequestDto photoRequestDto in requestDto.Photos)
            {
                CatalogItemPhoto photo = new CatalogItemPhoto
                {
                    Url = await _photoService.CreateAsync(
                        photoRequestDto.File,
                        "services",
                        false)
                };

                catalogItem.Photos.Add(photo);
            }
        }

        // Save changes.
        try
        {
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return catalogItem.Id;
        }
        catch (DbUpdateException exception)
        {
            // Delete the recently created thumbnail if existing.
            if (catalogItem.ThumbnailUrl != null)
            {
                _photoService.Delete(catalogItem.ThumbnailUrl);
            }

            // Delete the recently created photos if exsting.
            if (catalogItem.Photos?.Count > 0)
            {
                foreach (CatalogItemPhoto photo in catalogItem.Photos)
                {
                    _photoService.Delete(photo.Url);
                }
            }
            
            // Handle the concurrency-related operation.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, CatalogItemUpsertRequestDto requestDto)
    {
        // Use transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Fetch the entity in the database.
        CatalogItem catalogItem = await _context.CatalogItems
            .Include(bs => bs.Photos)
            .SingleOrDefaultAsync(bs => bs.Id == id)
            ?? throw new ResourceNotFoundException();

        // Update the entity's properties.
        catalogItem.Name = requestDto.Name;
        catalogItem.Summary = requestDto.Summary;
        catalogItem.Detail = requestDto.Detail;

        // Prepare lists of urls to be deleted later when the operation succeeds or fails.
        List<string> urlsToBeDeletedWhenFailure = new List<string>();
        List<string> urlsToBeDeletedWhenSuccess = new List<string>();

        // Replace the thumbnail with a new one if it has been changed.
        if (requestDto.ThumbnailChanged)
        {
            // Delete the old thumbnail with the URL stored in the entity property if exists
            if (catalogItem.ThumbnailUrl != null)
            {
                urlsToBeDeletedWhenSuccess.Add(catalogItem.ThumbnailUrl);
                catalogItem.ThumbnailUrl = null;
            }

            // Add a new thumbnail if the request contains it
            if (requestDto.ThumbnailFile != null)
            {
                catalogItem.ThumbnailUrl = await _photoService.CreateAsync(
                    requestDto.ThumbnailFile,
                    "courses",
                    true);
                urlsToBeDeletedWhenFailure.Add(catalogItem.ThumbnailUrl);
            }
        }
        
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
                        urlsToBeDeletedWhenSuccess.Add(photo.Url);
                        catalogItem.Photos.Remove(photo);
                        continue;
                    }
                }
                else if (!photoRequestDto.IsDeleted)
                {
                    // Create new photo if the request doesn't have id.
                    photo = new CatalogItemPhoto
                    {
                        Url = await _photoService.CreateAsync(
                            photoRequestDto.File,
                            "courses",
                            false),
                        ItemId = catalogItem.Id
                    };

                    _context.CatalogItemPhotos.Add(photo);
                    
                    // Mark the created photo to be deleted later if the transaction fails.
                    urlsToBeDeletedWhenFailure.Add(photo.Url);
                }
            }
        }

        // Save changes.
        try
        {
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // The operation succeeded, delete the photos marked to be deleted.
            foreach (string url in urlsToBeDeletedWhenSuccess)
            {
                _photoService.Delete(url);
            }
        }
        catch (DbUpdateException exception)
        {
            // Delete the recently added thumbnail and photos.
            foreach (string url in urlsToBeDeletedWhenFailure)
            {
                _photoService.Delete(url);
            }
            
            // Handle the concurrency-related operation.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        // Fetch the entity with the given id from the database and ensure it exists.
        CatalogItem catalogItem = await _context.CatalogItems
            .Include(bs => bs.Photos)
            .SingleOrDefaultAsync(bs => bs.Id == id)
            ?? throw new ResourceNotFoundException(
                nameof(CatalogItem),
                nameof(id),
                id.ToString());

        // Delete the entity.
        _context.CatalogItems.Remove(catalogItem);

        // Delete all photos.
        foreach (CatalogItemPhoto photo in catalogItem.Photos)
        {
            _context.CatalogItemPhotos.Remove(photo);
        }

        // Save changes.
        try
        {
            await _context.SaveChangesAsync();

            // The entities are deleted successfully, remove the photo files.
            foreach (CatalogItemPhoto photo in catalogItem.Photos)
            {
                _photoService.Delete(photo.Url);
            }
        }
        catch (DbUpdateException exception)
        {
            // Handle the concurrency-related exception.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }
}