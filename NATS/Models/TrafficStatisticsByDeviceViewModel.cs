namespace NATS.Models;

public class TrafficStatisticsByDeviceViewModel
{
    [Display(Name = "Tên thiết bị")]
    public string DeviceName { get; set; }

    [Display(Name = DisplayNames.AccessCount)]
    public int AccessCount { get; set; }
}