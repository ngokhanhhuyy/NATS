namespace NATS.Services.Entities;

public class ProductPrice
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int Quatity { get; set; }

    [Required]
    [StringLength(25)]
    public string Unit { get; set; }

    public int? Price { get; set; }

    // Foreign keys
    public int ProductId { get; set; }

    // Navigation properties
    public virtual Product Product { get; set; }
}