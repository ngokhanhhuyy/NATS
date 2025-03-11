namespace NATS.Services.Interfaces;

/// <summary>
/// A service to handle photo-related operations.
/// </summary>
public interface IPhotoService
{
    /// <summary>
    /// Creates a new photo and save it on the <c>wwwroot/images/data</c> directory.
    /// </summary>
    /// <remarks>
    /// The name of the photo will be the time when this method is called.
    /// </remarks>
    /// <param name="content">
    /// An array of byte representing the photo file after reading file from the request.
    /// </param>
    /// <param name="cropToSquare">
    /// (Optional, default: <c>false</c>) Determine if the image should be cropped into a
    /// square image.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the
    /// relative path (URL) to the created photo on the server.
    /// </returns>
    /// <example>
    /// await CreateAsync(photoBytes, false);
    /// => ~/images/data/{id}.jpg.
    /// </example>
    Task<string> CreateAsync(byte[] content, bool cropToSquare = false);

    /// <summary>
    /// Creates a new photo and save it on the <c>wwwroot/images/data</c> directory.
    /// </summary>
    /// <remarks>
    /// The name of the photo will be the time when this method is called.
    /// </remarks>
    /// <param name="content">
    /// An array of byte representing the photo file after reading file from the request.
    /// </param>
    /// <param name="aspectRatio">
    /// Determine the aspect ratio of the image after being processed.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is the
    /// relative path (URL) to the created photo on the server.
    /// </returns>
    /// <example>
    /// await CreateAsync(photoBytes, false);
    /// => ~/images/data/{id}.jpg.
    /// </example>
    Task<string> CreateAsync(byte[] content, double aspectRatio);
    
    /// <summary>
    /// Deletes an existing photo by the relative path on the server.
    /// </summary>
    /// <param name="relativePath">
    /// A <see cref="string"/> representing the full path to the photo on the server.
    /// </param>
    void Delete(string relativePath);
}