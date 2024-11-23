namespace NATS.Services.Entities;

public class TrafficByHourIPAddress
{
    [Column("id")]
    [Key]
    public int Id { get; set; }
    
    [Column("ip_address")]
    [StringLength(15)]
    public string IPAddress { get; set; }
    
    [Column("access_count")]
    [Required]
    public int AccessCount { get; set; }
    
    [Column("last_access_at")]
    [Required]
    public DateTime LastAccessAt { get; set; }
    
    [Column("last_user_agent")]
    [StringLength(10000)]
    public string LastUserAgent { get; set; }

    // Foreign key
    [Column("traffic_by_hour_id")]
    [Required]
    public int TrafficByHourId { get; set; }

    // Navigation properties
    public virtual TrafficByHour TrafficByHour { get; set; }
}