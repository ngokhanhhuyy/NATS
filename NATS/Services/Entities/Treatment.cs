namespace NATS.Services.Entities;

public class Treatment
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public int Name { get; set; }

    [StringLength(255)]
    public string Summary { get; set; }

    [StringLength(5000)]
    public string Detail { get; set; }

    [StringLength(3000)]
    public string SpecialDescription { get; set; }
    
    [StringLength(255)]
    public string ThumbnailUrl { get; set; }

    [Required]
    public bool IsPopular { get; set; }

    // Navigation properties
    public virtual List<TreatmentTarget> Targets { get; set; }
    public virtual List<TreatmentPhoto> Photos { get; set; }
}