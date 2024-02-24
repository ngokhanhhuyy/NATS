namespace NATS.Services.Entities;

public class Enquiry
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string FullName { get; set; }

    [Required]
    [StringLength(255)]
    public string ContactInformation { get; set; }

    [Required]
    [StringLength(1000)]
    public string Content { get; set; }

    [Required]
    public bool IsCompleted { get; set; }
}