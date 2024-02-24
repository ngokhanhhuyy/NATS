namespace NATS.Services.Entities;

public class TreatmentPhoto
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Url { get; set; }

    // Foreign keys
    [Required]
    public int TreatmentId { get; set; }

    // Navigation properties
    public virtual Treatment Treatment { get; set; }
}