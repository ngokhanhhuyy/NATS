namespace NATS.Services.Entities;

public class TeamMember
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("full_name")]
    [Required]
    [StringLength(50)]
    public string FullName { get; set; }
    
    [Column("role_name")]
    [Required]
    [StringLength(50)]
    public string RoleName { get; set; }

    [Column("description")]
    [Required]
    [StringLength(400)]
    public string Description { get; set; }

    [Column("photo_url")]
    [StringLength(255)]
    public string PhotoUrl { get; set; }
}