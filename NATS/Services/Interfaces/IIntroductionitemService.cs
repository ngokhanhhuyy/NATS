namespace NATS.Services.Interfaces;

public interface IIntroductionItemService
{
    Task<ServiceResult<List<IntroductionItemResponseDto>>> GetListAsync();

    Task<ServiceResult<IntroductionItemResponseDto>> GetAsync(int id);

    Task<ServiceResult<IntroductionItemResponseDto>> UpdateAsync(
        int id,
        IntroductionItemRequestDto requestDto);
}