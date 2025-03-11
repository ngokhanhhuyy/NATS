namespace NATS.Services.Dtos.RequestDtos;

public class SliderItemUpsertRequestDto
        : IHasThumbnailUpsertRequestDto<SliderItemUpsertRequestDto>
{
    public string Title { get; set; }
    public byte[] ThumbnailFile { get; set; }
    public bool ThumbnailChanged { get; set; }

    public SliderItemUpsertRequestDto TransformValues()
    {
        Title = Title.ToNullIfEmpty();
        return this;
    }
}