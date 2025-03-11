namespace NATS.Services.Entities;

public class SliderItem : IHasThumbnailEntity
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("title")]
    [StringLength(100)]
    public string Title { get; set; }

    [Column("thumbnail_url")]
    [StringLength(255)]
    public string ThumbnailUrl { get; set; }

    [Column("index")]
    [Required]
    public int Index { get; set; } = 0;
}