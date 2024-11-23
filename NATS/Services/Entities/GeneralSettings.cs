namespace NATS.Services.Entities;

public class GeneralSettings
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("application_name")]
    [Required]
    [StringLength(100)]
    public string ApplicationName { get; set; }

    [Column("application_short_name")]
    [Required]
    [StringLength(15)]
    public string ApplicationShortName { get; set; }

    [Column("fav_icon_url")]
    [StringLength(255)]
    public string FavIconUrl { get; set; }

    [Column("under_maintainance")]
    [Required]
    public bool UnderMaintainance { get; set; } = false;
}
