namespace NATS.Services.Interfaces;

/// <summary>
/// A service to handle photo-related operations.
/// </summary>
public interface IPhotoService
{
    /// <summary>
    /// Creates a new photo and save it on the photo folder with subfolder having the name
    /// that is specified by <paramref name="folderName"/>.
    /// </summary>
    /// <remarks>
    /// The name of the photo will be the time when this method is called.
    /// </remarks>
    /// <param name="content">
    /// An array of byte representing the photo file after reading file from the request.
    /// </param>
    /// <param name="folderName">
    /// The name of the folder inside <c>/images/front-pages</c> directory that this photo will
    /// be saved.
    /// </param>
    /// <param name="cropToSquare">
    /// Determine if the image should be cropped into square image.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the
    /// relative path (URL) to the created photo on the server.
    /// </returns>
    /// <example>
    /// await CreateAsync(photoBytes, "users", false);
    /// => ~/photos/users/{id}.jpg.
    /// </example>
    Task<string> CreateAsync(byte[] content, string folderName, bool cropToSquare = false);

    /// <summary>
    /// Creates a new photo and save it on the photo folder with subfolder having the name
    /// that is specified by <paramref name="folderName"/>.
    /// </summary>
    /// <remarks>
    /// The name of the photo will be the time when this method is called.
    /// </remarks>
    /// <param name="content">
    /// An array of byte representing the photo file after reading file from the request.
    /// </param>
    /// <param name="folderName">
    /// The name of the folder inside <c>/images/front-pages</c> directory that this photo will
    /// be saved.
    /// </param>
    /// <param name="aspectRatio">
    /// Determine the aspect ratio of the image after being processed.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the
    /// relative path (URL) to the created photo on the server.
    /// </returns>
    /// <example>
    /// await CreateAsync(photoBytes, "users", 1.5);
    /// => ~/photos/users/{id}.jpg.
    /// </example>
    Task<string> CreateAsync(byte[] content, string folderName, double aspectRatio);
    
    /// <summary>
    /// Deletes an existing photo by the relative path on the server.
    /// </summary>
    /// <param name="relativePath">
    /// A <see cref="string"/> representing the full path to the photo on the server, usually
    /// in <c>/wwwroot/photos/{entityName}/</c>
    /// </param>
    void Delete(string relativePath);
}