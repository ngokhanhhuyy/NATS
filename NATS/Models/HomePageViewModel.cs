namespace NATS.Models;

public class HomePageViewModel
{
    [Display(Name = DisplayNames.HomePageSliderItem)]
    public HomePageSliderItemListViewModel HomePageSliderItems { get; set; }

    [Display(Name = DisplayNames.Introduction)]
    public IntroductionItemListViewModel IntroductionItems { get; set; }
    
    [Display(Name = DisplayNames.Course)]
    public CourseBasicListViewModel Courses { get; set; }
    
    [Display(Name = DisplayNames.BusinessService)]
    public BusinessServiceBasicListViewModel BusinessServices { get; set; }
    
    [Display(Name = DisplayNames.Product)]
    public ProductBasicListViewModel Products { get; set; }
}