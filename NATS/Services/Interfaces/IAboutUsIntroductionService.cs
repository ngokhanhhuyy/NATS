namespace NATS.Services.Interfaces;

public interface IAboutUsIntroductionService
{
    Task<ServiceResult<AboutUsIntroductionResponseDto>> GetAsync();

    Task<ServiceResult<AboutUsIntroductionResponseDto>> UpdateAsync(
            AboutUsIntroductionRequestDto requestDto);
}