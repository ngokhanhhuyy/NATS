namespace NATS.Models;

public class HomePageModel
{
    [Display(Name = DisplayNames.SliderItem)]
    public List<SliderItemDetailModel> SliderItems { get; set; }

    [Display(Name = DisplayNames.Introduction)]
    public List<SummaryItemDetailModel> SummaryItems { get; set; }
    
    [Display(Name = DisplayNames.Course)]
    public List<CatalogItemBasicModel> Courses { get; set; }
    
    [Display(Name = DisplayNames.Service)]
    public List<CatalogItemBasicModel> Services { get; set; }
    
    [Display(Name = DisplayNames.Product)]
    public List<CatalogItemBasicModel> Products { get; set; }

    public HomePageModel(
            List<SliderItemResponseDto> sliderItemResponseDtos,
            List<SummaryItemResponseDto> summaryItemResponseDtos,
            List<CatalogItemBasicResponseDto> courseResponseDtos,
            List<CatalogItemBasicResponseDto> serviceResponseDtos,
            List<CatalogItemBasicResponseDto> productResponseDtos)
    {
        SliderItems = sliderItemResponseDtos
            .Select(dto => new SliderItemDetailModel(dto))
            .ToList();
        SummaryItems = summaryItemResponseDtos
            .Select(dto => new SummaryItemDetailModel(dto))
            .ToList();
        Courses = courseResponseDtos
            .Select(dto => new CatalogItemBasicModel(dto))
            .ToList();
        Services = serviceResponseDtos
            .Select(dto => new CatalogItemBasicModel(dto))
            .ToList();
        Products = productResponseDtos
            .Select(dto => new CatalogItemBasicModel(dto))
            .ToList();
    }
}