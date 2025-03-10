namespace NATS.Services.Dtos.RequestDtos;

public class SliderItemUpsertRequestDto : IRequestDto<SliderItemUpsertRequestDto>
{
    public string Title { get; set; }
    public byte[] PhotoFile { get; set; }
    public bool PhotoChanged { get; set; }

    public SliderItemUpsertRequestDto TransformValues()
    {
        Title = Title.ToNullIfEmpty();
        return this;
    }
}