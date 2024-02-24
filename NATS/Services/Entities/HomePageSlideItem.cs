namespace NATS.Services.Entities;

public class HomePageSlideItem
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("title")]
    [StringLength(100)]
    public string Title { get; set; }

    [Column("content")]
    [StringLength(100)]
    public string Content { get; set; }

    [Column("photo_url")]
    [StringLength(255)]
    public string PhotoUrl { get; set; }

    [Column("index")]
    [Required]
    public string Index { get; set; }
}