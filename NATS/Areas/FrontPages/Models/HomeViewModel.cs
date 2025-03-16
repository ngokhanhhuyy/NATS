namespace NATS.FrontPages.Models;

public class HomeViewModel
{
    public List<SliderItemDetailModel> SliderItems { get; set; }
    public List<SummaryItemDetailModel> SummaryItems { get; set; }
    public AboutUsIntroductionDetailModel AboutUsIntroduction { get; set; }
    public List<CatalogItemBasicModel> Services { get; set; }
    public List<CatalogItemBasicModel> Courses { get; set; }
    public List<CatalogItemBasicModel> Products { get; set; }
    public List<ContactDetailModel> Contacts { get; set; }
    public GeneralSettingsDetailModel GeneralSettings { get; set; }
}