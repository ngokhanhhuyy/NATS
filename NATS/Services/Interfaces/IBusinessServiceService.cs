namespace NATS.Services.Interfaces;

public interface IBusinessServiceService
{
    Task<ServiceResult<List<BusinessServiceBasicResponseDto>>> GetBasicListAsync();

    Task<ServiceResult<List<BusinessServiceDetailResponseDto>>> GetDetailListAsync();

    Task<ServiceResult<BusinessServiceDetailResponseDto>> GetAsync(int id);

    Task<ServiceResult<BusinessServiceDetailResponseDto>> CreateAsync(
        BusinessServiceRequestDto requestDto);

    Task<ServiceResult<BusinessServiceDetailResponseDto>> UpdateAsync(
        int id,
        BusinessServiceRequestDto requestDto);

    Task<ServiceResult<int>> DeleteAsync(int id);
}