namespace NATS.Services.Entities;

public class TreatmentTarget
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Description { get; set; }

    // Foreign keys
    [Required]
    public int TreatmentId { get; set; }

    // Navigation properties
    public virtual Treatment Treatment { get; set; }
}