namespace NATS.Services.Entities;

public class Contact : IEntity
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("type")]
    [Required]
    public ContactType Type { get; set; }

    [Column("content")]
    [Required]
    [StringLength(255)]
    public string Content { get; set; }
}
