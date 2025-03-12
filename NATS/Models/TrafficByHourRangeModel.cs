namespace NATS.Models;

public class TrafficByHourRangeModel
{
    [Display(Name = DisplayNames.PeriodOfDayName)]
    public string PeriodOfDayName { get; set; }
    
    [Display(Name = "Từ")]
    [DisplayFormat(DataFormatString = "{HH:mm}")]
    public TimeOnly FromTime { get; set; }
    
    [Display(Name = "Tới")]
    [DisplayFormat(DataFormatString = "{HH:mm}")]
    public TimeOnly ToTime { get; set; }
    
    [Display(Name = DisplayNames.AccessCount)]
    public int AccessCount { get; set; }
    
    [Display(Name = DisplayNames.GuestCount)]
    public int GuestCount { get; set; }

    public TrafficByHourRangeModel(TrafficByHourRangeResponseDto responseDto)
    {
        PeriodOfDayName = responseDto.PeriodOfDayName;
        FromTime = responseDto.FromTime;
        ToTime = responseDto.ToTime;
        AccessCount = responseDto.AccessCount;
        GuestCount = responseDto.GuestCount;
    }
}