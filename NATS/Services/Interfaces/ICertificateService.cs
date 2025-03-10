namespace NATS.Services.Interfaces;

public interface IBusinessCertificateService
{
    Task<ServiceResult<List<BusinessCertificateResponseDto>>> GetListAsync();

    Task<ServiceResult<BusinessCertificateResponseDto>> GetAsync(int id);

    Task<ServiceResult<BusinessCertificateResponseDto>> CreateAsync(
        BusinessCertificateRequestDto requestDto);

    Task<ServiceResult<BusinessCertificateResponseDto>> UpdateAsync(
        int id,
        BusinessCertificateRequestDto requestDto);

    Task<ServiceResult<int>> DeleteAsync(int id);
}