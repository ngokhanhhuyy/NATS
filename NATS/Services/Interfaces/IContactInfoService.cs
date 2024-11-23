namespace NATS.Services.Interfaces;

public interface IContactInfoService
{
    Task<ServiceResult<ContactInfoResponseDto>> GetAsync();

    Task<ServiceResult<ContactInfoResponseDto>> UpdateAsync(
        ContactInfoRequestDto requestDto);
}