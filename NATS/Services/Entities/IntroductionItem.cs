namespace NATS.Services.Entities;

public class IntroductionItem
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(25)]
    public string Name { get; set; }

    [Column("summary")]
    [StringLength(255)]
    public string Summary { get; set; }

    [Column("content")]
    [StringLength(3000)]
    public string Content { get; set; }

    [Column("thumbnail_url")]
    [StringLength(255)]
    public string ThumbnailUrl { get; set; }

    [Column("type")]
    public ItemType Type { get; set; }
}