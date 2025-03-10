namespace NATS.Services.Entities;

public class CatalogItemPhoto
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("url")]
    [Required]
    public string Url { get; set; }

    // Foreign keys
    [Column("item_id")]
    [Required]
    public int ItemId { get; set; }

    // Navigation property
    public virtual CatalogItem Item { get; set; }
}