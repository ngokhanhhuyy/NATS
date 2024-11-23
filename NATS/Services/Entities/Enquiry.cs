namespace NATS.Services.Entities;

public class Enquiry
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("full_name")]
    [Required]
    [StringLength(50)]
    public string FullName { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; }
    
    [Column("phone_number")]
    [Required]
    [StringLength(15)]
    public string PhoneNumber { get; set; }

    [Column("content")]
    [Required]
    [StringLength(1000)]
    public string Content { get; set; }
    
    [Column("received_datetime")]
    [Required]
    public DateTime ReceivedDateTime { get; set; } = DateTime.Now;

    [Column("is_completed")]
    [Required]
    public bool IsCompleted { get; set; } = false;
}