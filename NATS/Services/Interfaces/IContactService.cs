namespace NATS.Services.Interfaces;

public interface IContactInfoService
{
    Task<ContactResponseDto> GetListAsync();

    Task<ContactResponseDto> UpdateAsync(ContactUpsertRequestDto requestDto);
}