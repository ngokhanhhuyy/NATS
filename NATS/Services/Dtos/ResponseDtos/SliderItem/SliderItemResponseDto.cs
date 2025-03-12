namespace NATS.Services.Dtos.ResponseDtos;

public class SliderItemResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ThumbnailUrl { get; set; }
    public int Index { get; set; }

    public SliderItemResponseDto(SliderItem sliderItem)
    {
        Id = sliderItem.Id;
        Title = sliderItem.Title;
        ThumbnailUrl = sliderItem.ThumbnailUrl;
        Index = sliderItem.Index;
    }
}