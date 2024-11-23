namespace NATS.Models;

public class DashboardViewModel
{
    [Display(Name = DisplayNames.AccessCount)]
    [DisplayFormat(DataFormatString = "{0} lượt")]
    public int AccessCount { get; set; }
    
    [Display(Name = DisplayNames.GuessCount)]
    [DisplayFormat(DataFormatString = "{0} khách")]
    public int GuessCount { get; set; }
    
    [Display(Name = DisplayNames.Enquiry)]
    [DisplayFormat(DataFormatString = "{0} câu hỏi")]
    public int EnquiryCount { get; set; }
    
    [Display(Name = DisplayNames.User)]
    [DisplayFormat(DataFormatString = "{0} người")]
    public int UserCount { get; set; }

    public TrafficStatisticsByDateListViewModel TrafficStatisticsByDates { get; set; }

    public TrafficStatisticsByHourRangeListViewModel TrafficStatisticsByHourRanges { get; set; }

    public TrafficStatisticsByDeviceListViewModel TrafficStatisticsByDevices { get; set; }
}