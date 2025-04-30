namespace NATS.Services.Dtos;

public interface IHasThumbnailUpsertRequestDto : IRequestDto
{
    string ThumbnailUrl { get; set; }
}