namespace NATS.Services.Dtos.ResponseDtos;

public class TrafficStatisticsByDateResponseDto
{
    public DateTime RecordedDate { get; set; }
    public int AccessCount { get; set; }
    public int GuessCount { get; set; }
}