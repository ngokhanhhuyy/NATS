namespace NATS.Services.Dtos.ResponseDtos;

public class TrafficByDateResponseDto
{
    public DateOnly RecordedDate { get; set; }
    public int AccessCount { get; set; }
    public int GuestCount { get; set; }

    public TrafficByDateResponseDto(TrafficByDate traffic)
    {
        RecordedDate = traffic.RecordedDate;
        AccessCount = traffic.AccessCount;
        GuestCount = traffic.GuestCount;
    }
}