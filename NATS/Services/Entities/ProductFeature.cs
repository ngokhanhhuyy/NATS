namespace NATS.Services.Entities;

public class ProductFeature
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("content")]
    [Required]
    [StringLength(200)]
    public string Content { get; set; }

    // Foreign keys
    [Column("product_id")]
    [Required]
    public int ProductId { get; set; }

    // Navigation properties
    public virtual Product Product { get; set; }
}