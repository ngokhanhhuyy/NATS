namespace NATS.Services.Entities;

public class ProductFeature
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Description { get; set; }

    // Foreign keys
    [Required]
    public int ProductId { get; set; }

    // Navigation properties
    public virtual Product Product { get; set; }
}