namespace NATS.Services.Entities;

public class BusinessServicePhoto
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("url")]
    [Required]
    public string Url { get; set; }

    // Foreign keys
    [Column("business_service_id")]
    [Required]
    public int BusinessServiceId { get; set; }

    // Navigation property
    public virtual BusinessService BusinessService { get; set; }
}