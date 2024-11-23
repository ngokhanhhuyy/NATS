namespace NATS.Services.Entities;

public class ContactInfo
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("phone_number")]
    [StringLength(20)]
    public string PhoneNumber { get; set; }

    [Column("zalo_number")]
    [StringLength(20)]
    public string ZaloNumber { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; }

    [Column("address")]
    [StringLength(255)]
    public string Address { get; set; }
}
