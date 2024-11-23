namespace NATS.Models;

public class TrafficStatisticsByDateViewModel
{
    [Display(Name = DisplayNames.RecordedAt)]
    [DisplayFormat(DataFormatString = "{dd tháng MM yyyy}")]
    public DateTime RecordedDate { get; set; }

    [Display(Name = DisplayNames.AccessCount)]
    public int AccessCount { get; set; }
    
    [Display(Name = DisplayNames.GuessCount)]
    public int GuessCount { get; set; }
}