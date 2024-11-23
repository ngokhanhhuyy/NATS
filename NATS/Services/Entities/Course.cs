namespace NATS.Services.Entities;

public class Course
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("name")]
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Column("summary")]
    [StringLength(255)]
    public string Summary { get; set; }

    [Column("detail")]
    [StringLength(5000)]
    public string Detail { get; set; }
    
    [Column("thumbnail_url")]
    [StringLength(255)]
    public string ThumbnailUrl { get; set; }

    // Navigation properties
    public virtual List<CourseSection> Sections { get; set; }
    public virtual List<CoursePhoto> Photos { get; set; }
}