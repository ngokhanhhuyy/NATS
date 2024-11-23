namespace NATS.Services.Interfaces;

public interface IPostService
{
    Task<ServiceResult<PostBasicListResponseDto>> GetBasicListAsync(int page);

    Task<ServiceResult<List<PostBasicResponseDto>>> GetLastestBasicListAsync(int limit);

    Task<ServiceResult<List<PostDetailResponseDto>>> GetDetailListAsync();

    Task<ServiceResult<PostDetailResponseDto>> GetDetailAsync(int id, bool viewsIncrement = false);

    Task<ServiceResult<PostDetailResponseDto>> GetDetailAsync(
        string normalizedTitle,
        bool viewsIncrement);

    Task<ServiceResult<PostListStatisticsResponseDto>> GetStatisticsAsync();

    Task<ServiceResult<PostDetailResponseDto>> CreateAsync(
        PostDetailRequestDto requestDto);

    Task<ServiceResult<PostDetailResponseDto>> UpdateAsync(
        int id,
        PostDetailRequestDto requestDto);
    
    Task<ServiceResult<int>> DeleteAsync(int id);
}