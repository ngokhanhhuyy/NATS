namespace NATS.Services.Interfaces;

/// <summary>
/// A service to handle general-settings-related operations.
/// </summary>
public interface IGeneralSettingsService
{
    /// <summary>
    /// Gets the settings.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation, which result is a DTO
    /// containing the settings information.
    /// </returns>
    Task<GeneralSettingsResponseDto> GetAsync();

    /// <summary>
    /// Updates the settings.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the data for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{T}"/> representing the asynchronous operation.
    /// </returns>
    Task UpdateAsync(GeneralSettingsRequestDto requestDto);
}
