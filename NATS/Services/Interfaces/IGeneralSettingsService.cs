namespace NATS.Services.Interfaces;

public interface IGeneralSettingsService
{
    Task<ServiceResult<GeneralSettingsResponseDto>> GetAsync();

    Task<ServiceResult<GeneralSettingsResponseDto>> UpdateAsync(
            GeneralSettingsRequestDto requestDto);
}
