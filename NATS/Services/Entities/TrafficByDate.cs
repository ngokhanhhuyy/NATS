namespace NATS.Services.Entities;

public class TrafficByDate
{
    [Column("id")]
    [Key]
    public int Id { get; set; }
    
    [Column("recorded_at")]
    [Required]
    public DateTime RecordedAt { get; set; }
    
    [Column("access_count")]
    [Required]
    public int AccessCount { get; set; }
    
    [Column("guess_count")]
    [Required]
    public int GuessCount { get; set; }
    
    // Navigation properties
    public virtual List<TrafficByHour> TrafficByHours { get; set; }
}