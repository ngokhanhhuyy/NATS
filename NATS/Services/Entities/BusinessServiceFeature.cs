namespace NATS.Services.Entities;

public class BusinessServiceFeature
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("content")]
    [Required]
    [StringLength(200)]
    public string Content { get; set; }

    // Foreign keys
    [Column("business_service_id")]
    [Required]
    public int BusinessServiceId { get; set; }

    // Navigation properties
    public virtual BusinessService BusinessService { get; set; }
}