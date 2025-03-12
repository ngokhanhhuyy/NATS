namespace NATS.Models;

public interface IHasThumbnailUpsertModel
{
    string ThumbnailUrl { get; set; }
    IFormFile ThumbnailFile { get; set; }
    bool ThumbnailChanged { get; set; }
}