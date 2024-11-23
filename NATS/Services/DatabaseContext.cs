using NATS.Services.Entity;

namespace NATS.Services;

public class DatabaseContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>,
    IdentityUserRole<int>, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public DbSet<IntroductionItem> IntroductionItems { get; set; }
    public DbSet<AboutUsIntroduction> AboutUsIntroductions { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseSection> CourseSections { get; set; }
    public DbSet<CoursePhoto> CoursePhotos { get; set; }
    public DbSet<BusinessService> BusinessServices { get; set; }
    public DbSet<BusinessServiceFeature> BusinessServiceFeatures { get; set; }
    public DbSet<BusinessServicePhoto> BusinessServicePhotos { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductFeature> ProductFeatures { get; set; }
    public DbSet<ProductPrice> ProductPrices { get; set; }
    public DbSet<ProductPhoto> ProductPhotos { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }
    public DbSet<BusinessCertificate> BusinessCertificates { get; set; }
    public DbSet<Enquiry> Enquiries { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<TrafficByDate> TrafficByDates { get; set; }
    public DbSet<TrafficByHour> TrafficByHours { get; set; }
    public DbSet<TrafficByHourIPAddress> TrafficByHourIPAddresses { get; set; }
    public DbSet<GeneralSettings> GeneralSettings { get; set; }
    public DbSet<HomePageSliderItem> HomePageSliderItems { get; set; }
    public DbSet<ContactInfo> ContactInfos { get; set; }

    public DatabaseContext(DbContextOptions options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<HomePageSliderItem>(e =>
        {
            e.ToTable("homepage_slider_items");
            e.HasKey(si => si.Id);
            e.HasIndex(i => i.Index)
                .IsUnique()
                .HasDatabaseName("unique__homepage_slider_items__index");
        });
        builder.Entity<IntroductionItem>(e =>
        {
            e.ToTable("introduction_items");
            e.HasKey(ii => ii.Id);
        });
        builder.Entity<AboutUsIntroduction>(e =>
        {
            e.ToTable("about_us_introductions");
            e.HasKey(aui => aui.Id);
        });
        builder.Entity<BusinessService>(e => {
            e.ToTable("business_services");
            e.HasKey(bs => bs.Id);
        });
        builder.Entity<BusinessServiceFeature>(e =>
        {
            e.ToTable("business_service_features");
            e.HasKey(bsf => bsf.Id);
            e.HasOne(bsf => bsf.BusinessService)
                .WithMany(bs => bs.Features)
                .HasForeignKey(bsf => bsf.BusinessServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<BusinessServicePhoto>(e =>
        {
            e.ToTable("business_service_photos");
            e.HasKey(bsp => bsp.Id);
            e.HasOne(bsp => bsp.BusinessService)
                .WithMany(bs => bs.Photos)
                .HasForeignKey(bsp => bsp.BusinessServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<Product>(e =>
        {
            e.ToTable("products");
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Name)
                .IsUnique()
                .HasDatabaseName("unique__products__name");
        });
        builder.Entity<ProductFeature>(e =>
        {
            e.ToTable("product_features");
            e.HasKey(pf => pf.Id);
            e.HasOne(pf => pf.Product)
                .WithMany(p => p.Features)
                .HasForeignKey(pf => pf.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<ProductPrice>(e =>
        {
            e.ToTable("product_prices");
            e.HasKey(pp => pp.Id);
            e.HasOne(pp => pp.Product)
                .WithMany(p => p.Prices)
                .HasForeignKey(pp => pp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<ProductPhoto>(e =>
        {
            e.ToTable("product_photos");
            e.HasKey(pp => pp.Id);
            e.HasOne(pp => pp.Product)
                .WithMany(p => p.Photos)
                .HasForeignKey(pp => pp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<Course>(e =>
        {
            e.ToTable("courses");
            e.HasKey(c => c.Id);
            e.HasIndex(p => p.Name)
                .IsUnique()
                .HasDatabaseName("unique__courses__name");
        });
        builder.Entity<CourseSection>(e =>
        {
            e.ToTable("course_sections");
            e.HasKey(cs => cs.Id);
            e.HasOne(cs => cs.Course)
                .WithMany(c => c.Sections)
                .HasForeignKey(cs => cs.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<CoursePhoto>(e => {
            e.ToTable("course_photos");
            e.HasKey(cs => cs.Id);
            e.HasOne(cs => cs.Course)
                .WithMany(c => c.Photos)
                .HasForeignKey(cs => cs.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<TeamMember>(e =>
        {
            e.ToTable("team_members");
            e.HasKey(tm => tm.Id);
        });
        builder.Entity<BusinessCertificate>(e =>
        {
            e.ToTable("business_certificates");
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
            e.HasIndex(td => td.RecordedAt)
                .IsUnique()
                .HasDatabaseName("unique__traffic_by_date__recorded_at");
        });
        builder.Entity<TrafficByHour>(e =>
        {
            e.ToTable("traffic_by_hour");
            e.HasKey(th => th.Id);
            e.HasIndex(th => th.RecordedAt)
                .IsUnique()
                .HasDatabaseName("unique__traffic_by_hour__recoreded_at");
            e.HasOne(th => th.TrafficByDate)
                .WithMany(td => td.TrafficByHours)
                .HasForeignKey(th => th.TrafficByDateId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<TrafficByHourIPAddress>(e =>
        {
            e.ToTable("traffic_by_hour_ip_address");
            e.HasKey(thia => thia.Id);
            e.HasOne(thia => thia.TrafficByHour)
                .WithMany(th => th.IPAddresses)
                .HasForeignKey(thia => thia.TrafficByHourId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(thia => thia.LastAccessAt);
        });
        builder.Entity<GeneralSettings>(e =>
        {
            e.ToTable("general_settings");
            e.HasKey(gs => gs.Id);
        });
        builder.Entity<ContactInfo>(e =>
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