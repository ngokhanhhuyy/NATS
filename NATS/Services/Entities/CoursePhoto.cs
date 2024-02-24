namespace NATS.Services.Entities;

public class CoursePhoto
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("url")]
    [Required]
    public string Url { get; set; }

    // Foreign keys
    [Column("course_id")]
    [Required]
    public int CourseId { get; set; }

    // Navigation properties
    public virtual Course Course { get; set; }
}