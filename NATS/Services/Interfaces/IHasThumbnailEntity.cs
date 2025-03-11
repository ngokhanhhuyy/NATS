namespace NATS.Services.Interfaces;

public interface IHasThumbnailEntity : IEntity
{
    string ThumbnailUrl { get; set; }
}
