namespace NATS.Services.Entities;

public class Certificate : IHasThumbnailEntity
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; }

    [Column("photo_url")]
    [Required]
    [StringLength(255)]
    public string ThumbnailUrl { get; set; }
}