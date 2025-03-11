namespace NATS.Services.Dtos;

public interface IHasThumbnailUpsertRequestDto<TRequestDto> : IRequestDto<TRequestDto>
{
    byte[] ThumbnailFile { get; set; }
    bool ThumbnailChanged { get; set; }
}