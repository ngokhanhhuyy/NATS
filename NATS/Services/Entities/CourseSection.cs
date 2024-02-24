namespace NATS.Services.Entities;

public class CourseSection
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("content")]
    [Required]
    [StringLength(200)]
    public string Content { get; set; }

    // Foreign keys
    [Column("course_id")]
    [Required]
    public int CourseId { get; set; }

    // Navigation properties
    public virtual Course Course { get; set; }
}