using NATSInternal.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (environment == Environments.Development)
{
    builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
}
else
{
    builder.Services.AddControllersWithViews()
        .AddRazorRuntimeCompilation();
}

// Add signalR
builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddControllersWithViews();
string connectionString = builder.Configuration.GetConnectionString("MySQL");
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlite("Data Source=database.db");
});

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddErrorDescriber<VietnameseIdentityErrorDescriber>()
    .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 0;
});
builder.Services.ConfigureApplicationCookie(options => {
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.Events.OnSignedIn = async context =>
    {
        IUserService userService = context
            .HttpContext
            .RequestServices
            .GetService<IUserService>();

        bool parsable = int.TryParse(
            context.Principal!.FindFirstValue(ClaimTypes.NameIdentifier),
            out int userId);
        if (!parsable)
        {
            throw new InvalidOperationException();
        }
        await userService.SetCurrentUserId(userId);
        await Task.CompletedTask;
    };
});

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme);

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<SignInValidator>();
ValidatorOptions.Global.LanguageManager.Enabled = true;
ValidatorOptions.Global.LanguageManager = new ValidatorLanguageManager {
    Culture = new CultureInfo("vi")
};

// Dependency injection
builder.Services.AddScoped<SignInManager<User>>();
builder.Services.AddScoped<RoleManager<Role>>();
builder.Services.AddTransient<DatabaseContext>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGeneralSettingsService, GeneralSettingsService>();
builder.Services.AddScoped<IAboutUsIntroductionService, AboutUsIntroductionService>();
builder.Services.AddScoped<MemberService, MemberService>();
builder.Services.AddScoped<ICertificateService, CertificateService>();
builder.Services.AddScoped<ISummaryItemService, SummaryItemService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<ISummaryItemService, SummaryItemService>();
builder.Services.AddScoped<ICatalogItemService, CatalogItemService>();
builder.Services.AddScoped<ISliderItemService, SliderItemService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IEnquiryService, EnquiryService>();
builder.Services.AddScoped<IContactService, IContactService>();
builder.Services.AddScoped<ITrafficService, TrafficService>();

WebApplication app = builder.Build();
DataInitializer dataInitializer;
dataInitializer = new DataInitializer();
dataInitializer.InitializeData(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseDeveloperExceptionPage();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseMiddleware<TrafficMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<UnderMaintainanceMiddleware>();
app.UseMiddleware<CurrentUserMiddleware>();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}");
app.Run();