namespace NATS.Services.Entities;

public class HomePageSliderItem
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("title")]
    [StringLength(100)]
    public string Title { get; set; }

    [Column("photo_url")]
    [StringLength(255)]
    public string PhotoUrl { get; set; }

    [Column("index")]
    [Required]
    public int Index { get; set; } = 0;
}