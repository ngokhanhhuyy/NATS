namespace NATS.Models;

public class TrafficStatisticsByHourRangeViewModel
{
    [Display(Name = DisplayNames.Name)]
    public string Name { get; set; }
    
    [Display(Name = "Từ")]
    [DisplayFormat(DataFormatString = "{HH:mm}")]
    public TimeOnly FromTime { get; set; }
    
    [Display(Name = "Tới")]
    [DisplayFormat(DataFormatString = "{HH:mm}")]
    public TimeOnly ToTime { get; set; }
    
    [Display(Name = DisplayNames.AccessCount)]
    public int AccessCount { get; set; }
    
    [Display(Name = DisplayNames.GuessCount)]
    public int GuessCount { get; set; }
}