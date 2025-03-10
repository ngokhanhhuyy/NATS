namespace NATS.Services.Entities;

public class TrafficByDate
{
    [Column("id")]
    [Key]
    public int Id { get; set; }
    
    [Column("recorded_date")]
    [Required]
    public DateOnly RecordedDate { get; set; }
    
    [Column("access_count")]
    [Required]
    public int AccessCount { get; set; }
    
    [Column("guest_count")]
    [Required]
    public int GuestCount { get; set; }
    
    // Navigation properties
    public virtual List<TrafficByHour> TrafficByHours { get; set; }
}