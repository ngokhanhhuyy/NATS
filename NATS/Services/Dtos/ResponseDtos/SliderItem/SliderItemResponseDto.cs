namespace NATS.Services.Dtos.ResponseDtos;

public class SliderItemResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string PhotoUrl { get; set; }
    public int Index { get; set; }

    public SliderItemResponseDto(SliderItem sliderItem)
    {
        Id = sliderItem.Id;
        Title = sliderItem.Title;
        PhotoUrl = sliderItem.PhotoUrl;
        Index = sliderItem.Index;
    }
}