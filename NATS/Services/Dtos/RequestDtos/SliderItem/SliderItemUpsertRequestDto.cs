namespace NATS.Services.Dtos.RequestDtos;

public class SliderItemUpsertRequestDto : IHasThumbnailUpsertRequestDto
{
    public string Title { get; set; }
    public byte[] ThumbnailFile { get; set; }
    public bool ThumbnailChanged { get; set; }

    public void TransformValues()
    {
        Title = Title.ToNullIfEmpty();
    }
}