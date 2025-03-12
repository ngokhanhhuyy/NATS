namespace NATS.Models;

public class TrafficByDeviceModel
{
    [Display(Name = "Tên thiết bị")]
    public string DeviceName { get; set; }

    [Display(Name = DisplayNames.AccessCount)]
    public int AccessCount { get; set; }

    public TrafficByDeviceModel(TrafficByDeviceResponseDto responseDto)
    {
        DeviceName = responseDto.DeviceName;
        AccessCount = responseDto.AccessCount;
    }
}