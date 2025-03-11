namespace NATS.Services.Entities;

public class SummaryItem : IHasThumbnailEntity
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("name")]
    [Required]
    [StringLength(25)]
    public string Name { get; set; }

    [Column("summary_content")]
    [Required]
    [StringLength(255)]
    public string SummaryContent { get; set; }

    [Column("detail_content")]
    [Required]
    [StringLength(3000)]
    public string DetailContent { get; set; }

    [Column("thumbnail_url")]
    [StringLength(255)]
    public string ThumbnailUrl { get; set; }
}