namespace NATS.Services;

/// <summary>
/// An abstract service class to handle the operations related to the entities which can be
/// created, updated or deleted.
/// </summary>
/// <typeparam name="TEntity">
/// The type of the entity.
/// </typeparam>
/// <typeparam name="TUpsertRequestDto">
/// The type of the DTO which contains the data for the operations against the
/// <typeparamref name="TEntity"/> entity.
/// </typeparam>
public abstract class AbstractUpsertableService<TEntity, TUpsertRequestDto>
        where TEntity : class, IEntity, new()
        where TUpsertRequestDto : class, IRequestDto<TUpsertRequestDto>, new()
{
    protected DatabaseContext Context { get; init; }

    protected AbstractUpsertableService(DatabaseContext context)
    {
        Context = context;
    }

    /// <summary>
    /// Adds a created entity into its repository and performs saving to database operation.
    /// </summary>
    /// <remarks>
    /// Right after saving successfully, the method <see cref="HandleSuccessfulOperation"/>
    /// will be called. Otherwise, <see cref="HandleFailedOperation"/> will be called instead.
    /// Override these methods if needed to provide handling logic.
    /// </remarks>
    /// <param name="entity">
    /// An initialized entity to save into the database.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the id
    /// of the created entity.
    /// </returns>
    protected virtual async Task<int> SaveCreatedEntityAsync(
            TEntity entity,
            TUpsertRequestDto requestDto)
    {
        // Add the entity into the database.
        GetRepository(Context).Add(entity);

        // Save changes.
        try
        {
            await Context.SaveChangesAsync();

            // The operation is success, execute the handling logic for successful screnario.
            HandleSuccessfulOperation();

            return entity.Id;
        }
        catch
        {
            // The operation is failed, execute the handling logic for failure screnario.
            HandleFailedOperation();

            throw;
        }
    }

    /// <summary>
    /// Performs the operation which saves an updated entity into the database and handle
    /// exceptions if occuring.
    /// </summary>
    /// <remarks>
    /// Right after saving successfully, the method <see cref="HandleSuccessfulOperation"/>
    /// will be called. Otherwise, <see cref="HandleFailedOperation"/> will be called instead.
    /// Override these methods if needed to provide handling logic.
    /// </remarks>
    /// <param name="entity">
    /// An updated entity to be saved.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency-related conflict occuring during the operation.
    /// </exception>
    protected virtual async Task SaveUpdatedEntityAsync(
            TEntity entity,
            TUpsertRequestDto requestDto)
    {
        // Save changes.
        try
        {
            await Context.SaveChangesAsync();

            // The operation is successful, execute the logic for successful screnario.
            HandleSuccessfulOperation();
        }
        catch (DbUpdateException exception)
        {
            // The operation is failed, execute the logic for failure screnario.
            HandleFailedOperation();

            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            throw;
        }

    }
    
    /// <summary>
    /// Deletes an existing entity from its repository and performs saving operation..
    /// </summary>
    /// <remarks>
    /// Right after saving successfully, the method <see cref="HandleSuccessfulOperation"/>
    /// will be called. Otherwise, <see cref="HandleFailedOperation"/> will be called instead.
    /// Override these methods if needed to provide handling logic.
    /// </remarks>
    /// <param name="entity">
    /// The entity to delete.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency-related conflict occuring during the operation.
    /// </exception>
    protected virtual async Task SaveDeletedEntityAsync(TEntity entity)
    {
        // Performing the deleting operation.
        GetRepository(Context).Remove(entity);

        // Save changes.
        try
        {
            await Context.SaveChangesAsync();

            // The operation is successful, execute the logic for successful screnario.
            HandleSuccessfulOperation();
        }
        catch (DbUpdateException exception)
        {
            // The operation is successful, execute the logic for successful screnario.
            HandleFailedOperation();

            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }

    /// <summary>
    /// Gets the repository for the queries which are related to the
    /// <typeparamref name="TEntity"/> entities.
    /// </summary>
    /// <param name="context">
    /// An instance of the <see cref="DatabaseContext"/> class where the repository can be
    /// picked from.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="DbSet{T}"/> of <typeparamref name="TEntity"/>,
    /// representing the repository.
    /// </returns>
    protected abstract DbSet<TEntity> GetRepository(DatabaseContext context);

    /// <summary>
    /// Generates an instance of the <see cref="ResourceNotFoundException"/> class, which
    /// contains the error message based on the specified id.
    /// </summary>
    /// <param name="id">
    /// The id of the resource.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="ResourceNotFoundException"/> class.
    /// </returns>
    protected ResourceNotFoundException GetResourceNotFoundExceptionById(int id)
    {
        return new ResourceNotFoundException(typeof(TEntity).Name, nameof(id), id.ToString());
    }

    /// <summary>
    /// Provides logic which is executed after a creating or updating operation is successful.
    /// </summary>
    protected virtual void HandleSuccessfulOperation()
    {
    }
    
    /// <summary>
    /// Provides logic which is executed after a creating or updating operation is failed.
    /// </summary>
    protected virtual void HandleFailedOperation()
    {
    }
}
