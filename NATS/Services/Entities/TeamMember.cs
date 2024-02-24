namespace NATS.Services.Entities;

public class TeamMember
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string FullName { get; set; }
    
    [Required]
    [StringLength(50)]
    public string RoleName { get; set; }

    [Required]
    [StringLength(400)]
    public string Description { get; set; }

    [StringLength(255)]
    public string PhotoUrl { get; set; }
}