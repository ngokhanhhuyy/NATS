namespace NATS.Services.Entities;

public class IntroductionItem
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("name")]
    [Required]
    [StringLength(25)]
    public string Name { get; set; }

    [Column("summary")]
    [Required]
    [StringLength(255)]
    public string Summary { get; set; }

    [Column("content")]
    [Required]
    [StringLength(3000)]
    public string Content { get; set; }

    [Column("thumbnail_url")]
    [StringLength(255)]
    public string ThumbnailUrl { get; set; }
}