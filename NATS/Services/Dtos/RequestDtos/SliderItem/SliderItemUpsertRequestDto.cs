namespace NATS.Services.Dtos.RequestDtos;

public class SliderItemUpsertRequestDto : IHasThumbnailUpsertRequestDto
{
    public string Title { get; set; }
    public string ThumbnailUrl { get; set; }

    public void TransformValues()
    {
        Title = Title.ToNullIfEmpty();
        ThumbnailUrl = ThumbnailUrl.ToNullIfEmpty();
    }
}