namespace NATS.Services.Entities;

public class Post
{
    [Column("id")]
    [Key]
    public int Id { get; set; }
    
    [Column("title")]
    [Required]
    [StringLength(150)]
    public string Title { get; set; }
    
    [Column("normalized_title")]
    [Required]
    [StringLength(255)]
    public string NormalizedTitle { get; set; }
    
    [Column("thumbnail_url")]
    [StringLength(255)]
    public string ThumbnailUrl { get; set;}
    
    [Column("content")]
    [Required]
    [MaxLength(10000)]
    public string Content { get; set; }
    
    [Column("created_datetime")]
    [Required]
    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    
    [Column("updated_datetime")]
    public DateTime? UpdatedDateTime { get; set; }
    
    [Column("is_pinned")]
    [Required]
    public bool IsPinned { get; set; } = false;
    
    [Column("is_published")]
    [Required]
    public bool IsPublished { get; set; } = false;
    
    [Column("views")]
    [Required]
    public int Views { get; set; } = 0;
    
    // Foreign keys
    [Column("user_id")]
    [Required]
    public int UserId { get; set; }
    
    // Navigation properties
    public User User { get; set; }
}