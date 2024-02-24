namespace NATS.Services.Entities;

public class ProductPhoto
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Url { get; set; }

    // Foreign keys
    [Required]
    public int ProductId { get; set; }

    // Navigation properties
    public virtual Product Product { get; set; }
}