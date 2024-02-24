namespace NATS.Services.Entities;

public class Role : IdentityRole<int>
{
    [Column("display_name")]
    [Required]
    [StringLength(50)]
    public string DisplayName { get; set; }

    // Navigation properties
    public virtual List<User> Users { get; set; }
}
