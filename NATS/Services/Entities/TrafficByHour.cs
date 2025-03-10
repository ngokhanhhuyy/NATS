namespace NATS.Services.Entities;

public class TrafficByHour
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("recorded_datetime")]
    [Required]
    public DateTime RecordedDateTime { get; set; }

    [Column("access_count")]
    [Required]
    public int AccessCount { get; set; }
    
    [Column("guest_count")]
    [Required]
    public int GuestCount { get; set; }
    
    // Foreign key
    [Column("traffic_by_date_id")]
    [Required]
    public int TrafficByDateId { get; set; }

    // Navigation properties
    public virtual TrafficByDate TrafficByDate { get; set; }
    public virtual List<TrafficByHourIpAddress> IPAddresses { get; set; }
}