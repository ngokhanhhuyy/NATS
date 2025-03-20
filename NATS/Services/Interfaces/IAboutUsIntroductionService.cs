namespace NATS.Services.Interfaces;

/// <summary>
/// A service to handle the operations which are related to about us introduction.
/// </summary>
public interface IAboutUsIntroductionService
{
    /// <summary>
    /// Gets the about us introduction.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a DTO
    /// containing the information of the about us introduction.
    /// </returns>
    Task<AboutUsIntroductionResponseDto> GetAsync();

    /// <summary>
    /// Update the about us introduction.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the data for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Throws when there is a concurreny-related conflict occuring during the operation.
    /// </exception>
    Task UpdateAsync(AboutUsIntroductionUpdateRequestDto requestDto);
}