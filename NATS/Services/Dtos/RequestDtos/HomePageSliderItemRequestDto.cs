namespace NATS.Services.Dtos.RequestDtos;

public class HomePageSliderItemRequestDto : IRequestDto<HomePageSliderItemRequestDto>
{
    public string Title { get; set; }
    public byte[] PhotoFile { get; set; }
    public bool PhotoChanged { get; set; }

    public HomePageSliderItemRequestDto TransformValues()
    {
        Title = Title.ToNullIfEmpty();
        return this;
    }
}