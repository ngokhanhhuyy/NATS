namespace NATS.Protected.Models;

public class SliderItemUpsertViewModel
{
    [Display(Name = DisplayNames.Title)]
    public string Title { get; set; }

    [Display(Name = DisplayNames.Thumbnail)]
    public string ThumbnailUrl { get; set; }

    [Display(Name = "Thay đổi ảnh")]
    public IFormFile ThumbnailFile { get; set; }

    [Display(Name = "Đã thay đổi")]
    public bool ThumbnailChanged { get; set; }

    [Display(Name = "Trang tạo")]
    public bool IsForCreating { get; set; } = true;

    public void MapFromResponseDto(SliderItemResponseDto responseDto)
    {
        Title = responseDto.Title;
        ThumbnailUrl = responseDto.ThumbnailUrl;
        IsForCreating = false;
    }

    public async Task<SliderItemUpsertRequestDto> ToRequestDtoAsync()
    {
        byte[] thumbnailFile = null;
        if (ThumbnailFile != null)
        {
            using MemoryStream stream = new MemoryStream();
            await ThumbnailFile.CopyToAsync(stream);
            thumbnailFile = stream.ToArray();
        }
        
        return new SliderItemUpsertRequestDto
        {
            Title = Title,
            ThumbnailFile = thumbnailFile,
            ThumbnailChanged = ThumbnailChanged
        };
    }
}