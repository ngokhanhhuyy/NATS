namespace NATS.Services.Entities;

public class CatalogItem
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("name")]
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Column("type")]
    [Required]
    public CatalogItemType Type { get; set; }
    
    [Column("summary")]
    [StringLength(255)]
    public string Summary { get; set; }

    [Column("detail")]
    [StringLength(5000)]
    public string Detail { get; set; }

    [Column("thumbnail_url")]
    public string ThumbnailUrl { get; set; }

    // Navigation property
    public virtual List<CatalogItemPhoto> Photos { get; set; }
}
