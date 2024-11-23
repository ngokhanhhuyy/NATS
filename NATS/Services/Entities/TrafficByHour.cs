namespace NATS.Services.Entities;

public class TrafficByHour
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
    
    // Foreign key
    [Column("traffic_by_date_id")]
    [Required]
    public int TrafficByDateId { get; set; }

    // Navigation properties
    public virtual TrafficByDate TrafficByDate { get; set; }
    public virtual List<TrafficByHourIPAddress> IPAddresses { get; set; }
}