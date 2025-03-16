namespace NATS.Services;

public class DatabaseContext
    : IdentityDbContext<
        User,
        Role,
        int,
        IdentityUserClaim<int>,
        IdentityUserRole<int>,
        IdentityUserLogin<int>,
        IdentityRoleClaim<int>,
        IdentityUserToken<int>>
{
    public DbSet<SummaryItem> SummaryItems { get; set; }
    public DbSet<AboutUsIntroduction> AboutUsIntroductions { get; set; }
    public DbSet<CatalogItem> CatalogItems { get; set; }
    public DbSet<CatalogItemPhoto> CatalogItemPhotos { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Certificate> Certificates { get; set; }
    public DbSet<Enquiry> Enquiries { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<TrafficByDate> TrafficByDates { get; set; }
    public DbSet<TrafficByHour> TrafficByHours { get; set; }
    public DbSet<TrafficByHourIpAddress> TrafficByHourIpAddresses { get; set; }
    public DbSet<GeneralSettings> GeneralSettings { get; set; }
    public DbSet<SliderItem> SliderItems { get; set; }
    public DbSet<Contact> Contacts { get; set; }

    public DatabaseContext(DbContextOptions options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<SliderItem>(e =>
        {
            e.ToTable("slider_items");
            e.HasKey(si => si.Id);
            e.HasIndex(i => i.Index)
                .IsUnique()
                .HasDatabaseName("unique__slider_items__index");
        });
        builder.Entity<SummaryItem>(e =>
        {
            e.ToTable("summary_items");
            e.HasKey(ii => ii.Id);
        });
        builder.Entity<AboutUsIntroduction>(e =>
        {
            e.ToTable("about_us_introductions");
            e.HasKey(aui => aui.Id);
        });
        builder.Entity<CatalogItem>(e => {
            e.ToTable("catalog_items");
            e.HasKey(bs => bs.Id);
        });
        builder.Entity<CatalogItemPhoto>(e =>
        {
            e.ToTable("catalog_item_photos");
            e.HasKey(bsp => bsp.Id);
            e.HasOne(bsp => bsp.Item)
                .WithMany(bs => bs.Photos)
                .HasForeignKey(bsp => bsp.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<Member>(e =>
        {
            e.ToTable("members");
            e.HasKey(tm => tm.Id);
        });
        builder.Entity<Certificate>(e =>
        {
            e.ToTable("certificates");
            e.HasKey(bc => bc.Id);
        });
        builder.Entity<Enquiry>(e => {
            e.ToTable("enquiries");
            e.HasKey(enquiry => enquiry.Id);
        });
        builder.Entity<Post>(e =>
        {
            e.ToTable("posts");
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.NormalizedTitle)
                .IsUnique()
                .HasDatabaseName("unique__post__normalized_name");
        });
        builder.Entity<TrafficByDate>(e =>
        {
            e.ToTable("traffic_by_date");
            e.HasKey(td => td.Id);
            e.HasIndex(td => td.RecordedDate)
                .IsUnique()
                .HasDatabaseName("unique__traffic_by_date__recorded_date");
        });
        builder.Entity<TrafficByHour>(e =>
        {
            e.ToTable("traffic_by_hour");
            e.HasKey(th => th.Id);
            e.HasIndex(th => th.RecordedDateTime)
                .IsUnique()
                .HasDatabaseName("unique__traffic_by_hour__recoreded_datetime");
            e.HasOne(th => th.TrafficByDate)
                .WithMany(td => td.TrafficByHours)
                .HasForeignKey(th => th.TrafficByDateId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<TrafficByHourIpAddress>(e =>
        {
            e.ToTable("traffic_by_hour_ip_address");
            e.HasKey(thia => thia.Id);
            e.HasOne(thia => thia.TrafficByHour)
                .WithMany(th => th.IpAddresses)
                .HasForeignKey(thia => thia.TrafficByHourId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(thia => thia.LastAccessAt);
        });
        builder.Entity<GeneralSettings>(e =>
        {
            e.ToTable("general_settings");
            e.HasKey(gs => gs.Id);
        });
        builder.Entity<Contact>(e =>
        {
            e.ToTable("contact_info");
            e.HasKey(ci => ci.Id);
        });

        // Identity entities
        builder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(u => u.Id);
            e.Property(u => u.Id).HasColumnName("id");
            e.Property(u => u.UserName).HasColumnName("username");
            e.Property(u => u.AccessFailedCount).HasColumnName("access_failed_count");
            e.Property(u => u.ConcurrencyStamp).HasColumnName("concurrent_stamp");
            e.Property(u => u.Email).HasColumnName("email");
            e.Property(u => u.EmailConfirmed).HasColumnName("email_confirmed");
            e.Property(u => u.LockoutEnabled).HasColumnName("lockout_enabled");
            e.Property(u => u.LockoutEnd).HasColumnName("lockout_end");
            e.Property(u => u.NormalizedEmail).HasColumnName("normalized_email");
            e.Property(u => u.NormalizedUserName).HasColumnName("normalized_username");
            e.Property(u => u.PasswordHash).HasColumnName("password_hash");
            e.Property(u => u.PhoneNumber).HasColumnName("phone_number");
            e.Property(u => u.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
            e.Property(u => u.SecurityStamp).HasColumnName("security_stamp");
            e.Property(u => u.TwoFactorEnabled).HasColumnName("two_factor_enabled");
            e.Property(u => u.SecurityStamp).HasColumnName("security_stamp");
            e.HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<IdentityUserRole<int>>(ur => ur.ToTable("user_roles"));
            e.HasIndex(u => u.UserName)
                .IsUnique()
                .HasDatabaseName("unique__users__username");
        });
        builder.Entity<Role>(e =>
        {
            e.ToTable("roles");
            e.HasKey(r => r.Id);
            e.Property(r => r.Id).HasColumnName("id");
            e.Property(r => r.Name).HasColumnName("name");
            e.Property(r => r.NormalizedName).HasColumnName("normalized_name");
            e.Property(r => r.ConcurrencyStamp).HasColumnName("concurrent_stamp");
            e.HasIndex(r => r.Name)
                .IsUnique()
                .HasDatabaseName("unique__roles__name");
            e.HasIndex(r => r.DisplayName)
                .IsUnique()
                .HasDatabaseName("unique__roles__display_name");
        });
        builder.Entity<IdentityUserRole<int>>(e =>
        {
            e.ToTable("user_roles");
            e.Property(ur => ur.UserId).HasColumnName("user_id");
            e.Property(ur => ur.RoleId).HasColumnName("role_id");
        });
        builder.Entity<IdentityUserClaim<int>>(e =>
        {
            e.ToTable("user_claims");
            e.Property(uc => uc.Id).HasColumnName("id");
            e.Property(uc => uc.UserId).HasColumnName("user_id");
            e.Property(uc => uc.ClaimType).HasColumnName("claim_type");
            e.Property(uc => uc.ClaimValue).HasColumnName("claim_value");
        });
        builder.Entity<IdentityUserLogin<int>>(e =>
        {
            e.ToTable("user_logins");
            e.HasKey(ul => ul.UserId);
            e.Property(ul => ul.UserId).HasColumnName("user_id");
            e.Property(ul => ul.LoginProvider).HasColumnName("login_providers");
            e.Property(ul => ul.ProviderDisplayName).HasColumnName("provider_display_name");
            e.Property(ul => ul.ProviderKey).HasColumnName("provider_key");
        });
        builder.Entity<IdentityUserToken<int>>(e =>
        {
            e.ToTable("user_tokens");
            e.HasKey(ut => ut.UserId);
        });
        builder.Entity<IdentityRoleClaim<int>>(e =>
        {
            e.ToTable("role_claims");
            e.Property(rc => rc.Id).HasColumnName("id");
            e.Property(rc => rc.ClaimType).HasColumnName("claim_type");
            e.Property(rc => rc.ClaimValue).HasColumnName("claim_value");
            e.Property(rc => rc.RoleId).HasColumnName("role_id");
        });
    }
}