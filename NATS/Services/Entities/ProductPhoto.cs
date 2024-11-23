namespace NATS.Services.Entities;

public class ProductPhoto
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("url")]
    [Required]
    public string Url { get; set; }

    // Foreign keys
    [Column("product_id")]
    [Required]
    public int ProductId { get; set; }

    // Navigation properties
    public virtual Product Product { get; set; }
}