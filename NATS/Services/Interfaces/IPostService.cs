namespace NATS.Services.Interfaces;

/// <summary>
/// A service to handle post-related operations.
/// </summary>
public interface IPostService
{
    /// <summary>
    /// Gets a list of posts with basic information, based on the specified page and results
    /// per page conditions for filtering.
    /// </summary>
    /// <param name="page">
    /// The page of the posts to retrieve.
    /// </param>
    /// <param name="resultsPerPage">
    /// The number of posts per page to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a
    /// <see cref="List{T}"/> of DTOs, containing the basic information of the posts.
    /// </returns>
    Task<PostListResponseDto> GetListAsync(int page, int resultsPerPage = 15);

    /// <summary>
    /// Gets an existing post, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the post to retrieve.
    /// </param>
    /// <param name="viewsIncrement">
    /// Indicates that whether post's view should be incremented by 1 after it was successfully
    /// retrieved.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a DTO
    /// containing the detail information of the post.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the post, specified by <paramref name="id"/>, doesn't exist.
    /// </exception>
    Task<PostDetailResponseDto> GetDetailAsync(int id, bool viewsIncrement = false);

    /// <summary>
    /// Gets an existing post, specified by its normalized title.
    /// </summary>
    /// <param name="normalizedTitle">
    /// The normalized title of the post to retrieve.
    /// </param>
    /// <param name="viewsIncrement">
    /// Indicates that whether post's view should be incremented by 1 after it was successfully
    /// retrieved.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a DTO
    /// containing the detail information of the post.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the post, specified by <paramref name="normalizedTitle"/>, doesn't exist.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency-related conflict occuring during the operation.
    /// </exception> 
    Task<PostDetailResponseDto> GetDetailAsync(string normalizedTitle, bool viewsIncrement);

    /// <summary>
    /// Gets the statistics of posts (e.g TotalCount, TotalView, UnpublishedCount).
    /// </summary>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a DTO
    /// containing the statistics information.
    /// </returns>
    Task<PostStatsResponseDto> GetStatsAsync();

    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the data for the creating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a the
    /// id of the created post.
    /// </returns>
    Task<int> CreateAsync(PostUpsertRequestDto requestDto);

    /// <summary>
    /// Updates an existing post, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the post to update.
    /// </param>
    /// <param name="requestDto">
    /// A DTO containing the data for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the post, specified by <paramref name="id"/>, doesn't exist.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency-related conflict occuring during the operation.
    /// </exception> 
    Task UpdateAsync(int id, PostUpsertRequestDto requestDto);

    /// <summary>
    /// Deletes an existing post, specified by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the post to delete.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the post, specified by <paramref name="id"/>, doesn't exist.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurrency-related conflict occuring during the operation.
    /// </exception> 
    Task DeleteAsync(int id);
}