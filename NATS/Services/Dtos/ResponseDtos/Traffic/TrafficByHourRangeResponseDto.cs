namespace NATS.Services.Dtos.ResponseDtos;

public class TrafficByHourRangeResponseDto
{
    public string PeriodOfDayName { get; set; }
    public TimeOnly FromTime { get; set; }
    public TimeOnly ToTime { get; set; }
    public int AccessCount { get; set; }
    public int GuestCount { get; set; }
}