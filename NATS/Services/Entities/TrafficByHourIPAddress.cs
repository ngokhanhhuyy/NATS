namespace NATS.Services.Entities;

public class TrafficByHourIPAddress
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("last_access_at")]
    [Required]
    public DateTime LastAcessAt { get; set; }

    // Foreign key
    [Column("traffic_by_hour_id")]
    [Required]
    public int TrafficByHourId { get; set; }

    // Navigation properties
    public virtual TrafficByHour TrafficByHour { get; set; }
}