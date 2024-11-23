namespace NATS.Services.Dtos.ResponseDtos;

public class TrafficStatisticsByHourRangeResponseDto
{
    public string Name { get; set; }
    public TimeOnly FromTime { get; set; }
    public TimeOnly ToTime { get; set; }
    public int AccessCount { get; set; }
    public int GuessCount { get; set; }
}