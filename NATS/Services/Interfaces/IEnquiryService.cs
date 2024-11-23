namespace NATS.Services.Interfaces;

public interface IEnquiryService
{
    Task<ServiceResult<List<EnquiryResponseDto>>> GetListAsync();
    
    Task<ServiceResult<EnquiryResponseDto>> GetAsync(int id);
    
    Task<ServiceResult<int>> GetIncompletedCountAsync();
    
    Task<ServiceResult<int>> CreateAsync(EnquiryRequestDto requestDto);
    
    Task<ServiceResult<int>> MarkAsCompletedAsync(int id);
}