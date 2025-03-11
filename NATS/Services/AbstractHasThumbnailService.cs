namespace NATS.Services;

/// <summary>
/// An abstract class to provide the abstraction of the operations which are related to the
/// entities which have thumbnail.
/// </summary>
/// <typeparam name="TEntity">
/// The type of the entity which the operations are performed on.
/// </typeparam>
/// <typeparam name="TUpsertRequestDto">
/// The type of the DTO which contains the data for the creating and updating operations.
/// </typeparam>
public abstract class AbstractHasThumbnailService<TEntity, TUpsertRequestDto>
            : AbstractUpsertableService<TEntity, TUpsertRequestDto>
        where TEntity : class, IHasThumbnailEntity, new()
        where TUpsertRequestDto :
            class,
            IHasThumbnailUpsertRequestDto<TUpsertRequestDto>,
            new()
{
    private readonly IPhotoService _photoService;
    private List<string> _photoUrlsToBeDeletedWhenSuccess;
    private List<string> _photoUrlsToBeDeletedWhenFailure;

    protected AbstractHasThumbnailService(
            DatabaseContext context,
            IPhotoService photoService) : base(context)
    {
        _photoService = photoService;
    }

    /// <inheritdoc/>
    protected async override Task<int> SaveCreatedEntityAsync(
            TEntity entity,
            TUpsertRequestDto requestDto)
    {
        // Save the photo if exist.
        if (requestDto.ThumbnailFile != null)
        {
            byte[] thumbnailFile = requestDto.ThumbnailFile;
            entity.ThumbnailUrl = await _photoService.CreateAsync(thumbnailFile, true);
            _photoUrlsToBeDeletedWhenFailure ??= new List<string>();
            _photoUrlsToBeDeletedWhenFailure.Add(entity.ThumbnailUrl);
        }

        return await base.SaveCreatedEntityAsync(entity, requestDto);
    }

    /// <inheritdoc/>
    protected async override Task SaveUpdatedEntityAsync(
            TEntity entity,
            TUpsertRequestDto requestDto)
    {
        if (requestDto.ThumbnailChanged)
        {
            // Mark the current photo to be deleted later, if exists.
            if (entity.ThumbnailUrl != null)
            {
                entity.ThumbnailUrl = null;
                _photoUrlsToBeDeletedWhenSuccess ??= new List<string>();
                _photoUrlsToBeDeletedWhenSuccess.Add(entity.ThumbnailUrl);
            }

            // Create new photo if it's data is included in the request.
            if (requestDto.ThumbnailFile != null)
            {
                byte[] thumbnailFile = requestDto.ThumbnailFile;
                entity.ThumbnailUrl = await _photoService.CreateAsync(thumbnailFile, true);
                _photoUrlsToBeDeletedWhenFailure ??= new List<string>();
                _photoUrlsToBeDeletedWhenFailure.Add(entity.ThumbnailUrl);
            }
        }
    }

    /// <inheritdoc/>
    protected override async Task SaveDeletedEntityAsync(TEntity entity)
    {
        GetRepository(_context).Remove(entity);

        await base.SaveDeletedEntityAsync(entity);
    }

    /// <inheritdoc/>
    protected override void HandleSuccessfulOperation()
    {
        if (_photoUrlsToBeDeletedWhenSuccess != null)
        {
            foreach (string url in _photoUrlsToBeDeletedWhenSuccess)
            {
                _photoService.Delete(url);
            }
        }

        base.HandleSuccessfulOperation();
    }

    /// <inheritdoc/>
    protected override void HandleFailedOperation()
    {
        if (_photoUrlsToBeDeletedWhenFailure != null)
        {
            foreach (string url in _photoUrlsToBeDeletedWhenFailure)
            {
                _photoService.Delete(url);
            }
        }

        base.HandleFailedOperation();
    }
}
