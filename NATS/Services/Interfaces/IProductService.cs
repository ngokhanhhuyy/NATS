namespace NATS.Services.Interfaces;

public interface IProductService
{
    Task<ServiceResult<List<ProductBasicResponseDto>>> GetBasicListAsync();

    Task<ServiceResult<List<ProductDetailResponseDto>>> GetDetailListAsync();

    Task<ServiceResult<ProductDetailResponseDto>> GetAsync(int id);

    Task<ServiceResult<ProductDetailResponseDto>> CreateAsync(ProductRequestDto requestDto);

    Task<ServiceResult<ProductDetailResponseDto>> UpdateAsync(
        int id,
        ProductRequestDto requestDto);

    Task<ServiceResult<int>> DeleteAsync(int id);
}