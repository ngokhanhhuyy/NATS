namespace NATS.Services.Entities;

public class ProductPrice
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("quantity")]
    [Required]
    public int Quantity { get; set; }

    [Column("unit")]
    [Required]
    [StringLength(25)]
    public string Unit { get; set; }

    [Column("price")]
    public int? Price { get; set; }

    // Foreign keys
    [Column("product_id")]
    [Required]
    public int ProductId { get; set; }

    // Navigation properties
    public virtual Product Product { get; set; }
}