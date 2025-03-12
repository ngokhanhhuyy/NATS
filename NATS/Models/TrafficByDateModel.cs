namespace NATS.Models;

public class TrafficByDateModel
{
    [Display(Name = DisplayNames.RecordedDateTime)]
    [DisplayFormat(DataFormatString = "{dd tháng MM yyyy}")]
    public DateOnly RecordedDate { get; set; }

    [Display(Name = DisplayNames.AccessCount)]
    public int AccessCount { get; set; }
    
    [Display(Name = DisplayNames.GuestCount)]
    public int GuestCount { get; set; }

    public TrafficByDateModel(TrafficByDateResponseDto responseDto)
    {
        RecordedDate = responseDto.RecordedDate;
        AccessCount = responseDto.AccessCount;
        GuestCount = responseDto.GuestCount;
    }
}