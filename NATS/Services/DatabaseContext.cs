namespace NATS.Services;

public class DatabaseContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>,
    IdentityUserRole<int>, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{

    public DbSet<IntroductionItem> IntroductionItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductFeature> ProductFeatures { get; set; }
    public DbSet<ProductPrice> ProductPrices { get; set; }
    public DbSet<ProductPhoto> ProductPhotos { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseSection> CourseSections { get; set; }
    public DbSet<CoursePhoto> CoursePhotos { get; set; }
    public DbSet<Treatment> Treatments { get; set; }
    public DbSet<TreatmentTarget> TreatmentTargets { get; set; }
    public DbSet<TreatmentPhoto> TreatmentPhotos { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }

    public DatabaseContext(DbContextOptions options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<HomePageSlideItem>(e => {
            e.ToTable("homepage_slide_item");
            e.HasKey(si => si.Id);
        });
        builder.Entity<IntroductionItem>(e => {
            e.ToTable("introduction_items");
            e.HasKey(bs => bs.Id);
        });
        builder.Entity<Product>(e => {
            e.ToTable("products");
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.Name)
                .IsUnique()
                .HasDatabaseName("unique_products_name");
        });
        builder.Entity<ProductFeature>(e => {
            e.ToTable("product_features");
            e.HasKey(pf => pf.Id);
            e.HasOne(pf => pf.Product)
                .WithMany(p => p.Features)
                .HasForeignKey(pf => pf.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<ProductPrice>(e => {
            e.ToTable("product_prices");
            e.HasKey(pp => pp.Id);
            e.HasOne(pp => pp.Product)
                .WithMany(p => p.Prices)
                .HasForeignKey(pp => pp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<ProductPhoto>(e => {
            e.ToTable("product_photos");
            e.HasKey(pp => pp.Id);
            e.HasOne(pp => pp.Product)
                .WithMany(p => p.Photos)
                .HasForeignKey(pp => pp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<Course>(e => {
            e.ToTable("courses");
            e.HasKey(c => c.Id);
            e.HasIndex(p => p.Name)
                .IsUnique()
                .HasDatabaseName("unique_courses_name");
        });
        builder.Entity<CourseSection>(e => {
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
        builder.Entity<Treatment>(e => {
            e.ToTable("treatments");
            e.HasKey(t => t.Id);
            e.HasIndex(t => t.Name)
                .IsUnique()
                .HasDatabaseName("unique_treatments_name");
        });
        builder.Entity<TreatmentTarget>(e => {
            e.ToTable("treatment_targets");
            e.HasKey(tt => tt.Id);
            e.HasOne(tt => tt.Treatment)
                .WithMany(t => t.Targets)
                .HasForeignKey(tt => tt.TreatmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<TreatmentPhoto>(e => {
            e.ToTable("treatment_photos");
            e.HasKey(cs => cs.Id);
            e.HasOne(cs => cs.Treatment)
                .WithMany(c => c.Photos)
                .HasForeignKey(cs => cs.TreatmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<TeamMember>(e => {
            e.ToTable("team_members");
            e.HasKey(tm => tm.Id);
        });
        builder.Entity<Enquiry>(e => {
            e.ToTable("enquiries");
            e.HasKey(enquiry => enquiry.Id);
        });
        builder.Entity<TrafficByHour>(e => {
            e.ToTable("traffic_by_hour");
            e.HasKey(th => th.Id);
            e.HasIndex(th => new { th.RecordedAt });
        });
        builder.Entity<TrafficByHourIPAddress>(e => {
            e.ToTable("traffic_by_hour_ip_address");
            e.HasKey(thia => thia.Id);
            e.HasOne(thia => thia.TrafficByHour)
                .WithMany(th => th.IPAddresses)
                .HasForeignKey(thia => thia.TrafficByHourId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(thia => thia.LastAcessAt);
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
            e.Property(u => u.SecurityStamp).HasColumnName("security_stamp");
            e.Property(u => u.TwoFactorEnabled).HasColumnName("two_factor_enabled");
            e.Property(u => u.SecurityStamp).HasColumnName("security_stamp");
            e.HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<IdentityUserRole<int>>(ur => ur.ToTable("user_roles"));
            e.HasIndex(u => u.UserName)
                .IsUnique()
                .HasDatabaseName("unique_users_username");
        });
        builder.Entity<Role>(e => {
            e.ToTable("roles");
            e.HasKey(r => r.Id);
            e.Property(r => r.Id).HasColumnName("id");
            e.Property(r => r.Name).HasColumnName("name");
            e.Property(r => r.NormalizedName).HasColumnName("normalized_name");
            e.HasIndex(r => r.Name)
                .IsUnique()
                .HasDatabaseName("unique_roles_name");
            e.HasIndex(r => r.DisplayName)
                .IsUnique()
                .HasDatabaseName("unique_roles_display_name");
        });
        builder.Entity<IdentityUserRole<int>>(e => {
            e.ToTable("user_roles");
            e.Property(ur => ur.UserId).HasColumnName("user_id");
            e.Property(ur => ur.RoleId).HasColumnName("role_id");
        });
        builder.Entity<IdentityUserClaim<int>>().ToTable("user_claims");
        builder.Entity<IdentityUserLogin<int>>(e => {
            e.ToTable("user_logins");
            e.HasKey(ul => ul.UserId);
            e.Property(ul => ul.UserId).HasColumnName("user_id");
            e.Property(ul => ul.LoginProvider).HasColumnName("login_providers");
            e.Property(ul => ul.ProviderDisplayName).HasColumnName("provider_display_name");
            e.Property(ul => ul.ProviderKey).HasColumnName("provider_key");
        });
        builder.Entity<IdentityUserToken<int>>(e => {
            e.ToTable("user_tokens");
            e.HasKey(ut => ut.UserId);
        });
        builder.Entity<IdentityRoleClaim<int>>().ToTable("role_claims");
    }
}