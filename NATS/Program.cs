using Microsoft.AspNetCore.Authentication.JwtBearer;

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
Console.WriteLine(connectionString);
// connectionString = "Server=MYSQL8003.site4now.net;Database=db_aa5821_nats;Uid=aa5821_nats;Password=Huyy47b1";
builder.Services.AddDbContext<DatabaseContext>(options => options
    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
    .AddInterceptors(new VietnamTimeInterceptor()));

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
        IUserService userService = context.HttpContext.RequestServices.GetService<IUserService>();
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

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                IUserService userService = context.HttpContext
                    .RequestServices
                    .GetRequiredService<IUserService>();
                string userIdAsString = context.Principal!.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int userId = int.Parse(userIdAsString!);
                await userService.SetCurrentUserId(userId);
            }
        };
    });

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();
ValidatorOptions.Global.LanguageManager.Enabled = true;
ValidatorOptions.Global.LanguageManager = new ValidatorLanguageManager {
    Culture = new CultureInfo("vi")
};

// Dependency injection
builder.Services.AddScoped<SignInManager<User>>();
builder.Services.AddScoped<RoleManager<Role>>();
builder.Services.AddScoped<DatabaseContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGeneralSettingsService, GeneralSettingsService>();
builder.Services.AddScoped<IAboutUsIntroductionService, AboutUsIntroductionService>();
builder.Services.AddScoped<ITeamMembersService, TeamMembersService>();
builder.Services.AddScoped<IBusinessCertificateService, BusinessCertificateService>();
builder.Services.AddScoped<IIntroductionItemService, IntroductionItemService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<IIntroductionItemService, IntroductionItemService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IBusinessServiceService, BusinessServiceService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IHomePageSliderItemService, HomePageSliderItemService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IEnquiryService, EnquiryService>();
builder.Services.AddScoped<IContactInfoService, ContactInfoService>();
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