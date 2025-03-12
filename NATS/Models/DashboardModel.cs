namespace NATS.Models;

public class DashboardModel
{
    [Display(Name = DisplayNames.TodayAccessCount)]
    [DisplayFormat(DataFormatString = "{0} lượt")]
    public int TodayAccessCount { get; set; }
    
    [Display(Name = DisplayNames.TodayGuestCount)]
    [DisplayFormat(DataFormatString = "{0} khách")]
    public int TodayGuestCount { get; set; }
    
    [Display(Name = DisplayNames.IncompletedEnquiryCount)]
    [DisplayFormat(DataFormatString = "{0} câu hỏi")]
    public int IncompletedEnquiryCount { get; set; }
    
    [Display(Name = DisplayNames.User)]
    [DisplayFormat(DataFormatString = "{0} người")]
    public int UserCount { get; set; }

    [Display(Name = DisplayNames.TrafficByDate)]
    public List<TrafficByDateModel> TrafficByDates { get; set; }

    [Display(Name = DisplayNames.TrafficByHourRange)]
    public List<TrafficByHourRangeModel> TrafficByHourRanges { get; set; }

    [Display(Name = DisplayNames.TrafficByDevice)]
    public List<TrafficByDeviceModel> TrafficByDevices { get; set; }

    public DashboardModel(
            int incompletedEnquiryCount,
            int userCount,
            List<TrafficByDateResponseDto> trafficByDateResponseDtos,
            List<TrafficByHourRangeResponseDto> trafficByHourRangeResponseDtos,
            List<TrafficByDeviceResponseDto> trafficByDeviceResponseDtos)
    {
        TodayAccessCount = trafficByDateResponseDtos.LastOrDefault()?.AccessCount ?? 0;
        TodayGuestCount = trafficByDateResponseDtos.LastOrDefault()?.GuestCount ?? 0;
        IncompletedEnquiryCount = incompletedEnquiryCount;
        UserCount = userCount;
        TrafficByDates = trafficByDateResponseDtos
            .Select(dto => new TrafficByDateModel(dto))
            .ToList();
        TrafficByHourRanges = trafficByHourRangeResponseDtos
            .Select(dto => new TrafficByHourRangeModel(dto))
            .ToList();
        TrafficByDevices = trafficByDeviceResponseDtos
            .Select(dto => new TrafficByDeviceModel(dto))
            .ToList();
    }
}