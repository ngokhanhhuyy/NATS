namespace NATS.Services.Interfaces;

public interface ICourseService
{
    Task<ServiceResult<List<CourseBasicResponseDto>>> GetBasicListAsync();

    Task<ServiceResult<List<CourseDetailResponseDto>>> GetDetailListAsync();

    Task<ServiceResult<CourseDetailResponseDto>> GetAsync(int id);

    Task<ServiceResult<CourseDetailResponseDto>> CreateAsync(CourseRequestDto requestDto);

    Task<ServiceResult<CourseDetailResponseDto>> UpdateAsync(
        int id,
        CourseRequestDto requestDto);

    Task<ServiceResult<int>> DeleteAsync(int id);
}
