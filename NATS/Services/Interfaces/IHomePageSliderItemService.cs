namespace NATS.Services.Interfaces;

public interface IHomePageSliderItemService
{
    Task<ServiceResult<List<HomePageSliderItemResponseDto>>> GetListAsync();

    Task<ServiceResult<HomePageSliderItemResponseDto>> GetAsync(int id);

    Task<ServiceResult<HomePageSliderItemResponseDto>> CreateAsync(
        HomePageSliderItemRequestDto requestDto);

    Task<ServiceResult<HomePageSliderItemResponseDto>> UpdateAsync(
        int id,
        HomePageSliderItemRequestDto requestDto);

    Task<ServiceResult<int>> DeleteAsync(int id);
}