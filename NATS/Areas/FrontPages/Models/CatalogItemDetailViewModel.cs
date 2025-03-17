namespace NATS.FrontPages.Models;

public class CatalogItemDetailViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CatalogItemType Type { get; set; }
    public string Summary { get; set; }
    public string Detail { get; set; }
    public string ThumbnailUrl { get; set; }
    public List<CatalogItemDetailPhotoModel> Photos { get; set; }
    public List<CatalogItemBasicModel> OtherCatalogItems { get; set; }

    public CatalogItemDetailViewModel(
            CatalogItemDetailResponseDto responseDto,
            List<CatalogItemBasicResponseDto> otherResponseDtos)
    {
        Id = responseDto.Id;
        Name = responseDto.Name;
        Type = responseDto.Type;
        Summary = responseDto.Summary;
        Detail = responseDto.Detail;
        ThumbnailUrl = responseDto.ThumbnailUrl;
        Photos = responseDto.Photos
            .Select(dto => new CatalogItemDetailPhotoModel(dto))
            .ToList();
        OtherCatalogItems = otherResponseDtos
            .Select(dto => new CatalogItemBasicModel(dto))
            .ToList();
    }
}