namespace NATS.Services.Entities;

public class AboutUsIntroduction
{
    [Column("id")]
    [Key]
    public int Id { get; set; }
    
    [Column("main_photo_url")]
    [StringLength(255)]
    public string MainPhotoUrl { get; set; }

    [Column("main_quote_content")]
    [Required]
    [StringLength(1000)]
    public string MainQuoteContent { get; set; }

    [Column("about_us_content")]
    [Required]
    [StringLength(1500)]
    public string AboutUsContent { get; set; }

    [Column("why_choose_us_content")]
    [Required]
    [StringLength(1500)]
    public string WhyChooseUsContent { get; set; }

    [Column("our_difference_content")]
    [Required]
    [StringLength(1500)]
    public string OurDifferenceContent { get; set; }

    [Column("our_culture_content")]
    [Required]
    [StringLength(1500)]
    public string OurCultureContent { get; set; }
}