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
    public int AccesscCount { get; set; } = 0;

    // Navigation properties
    public virtual List<TrafficByHourIPAddress> IPAddresses { get; set; }
}