using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using NATS.Helpers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

// Add controllers.
IMvcBuilder mvcBuilder = builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    });

if (environment == Environments.Development)
{
    // mvcBuilder.AddRazorRuntimeCompilation();
}

// Add database context.
string connectionString = builder.Configuration.GetConnectionString("MySQL");
builder.Services.AddDbContextFactory<DatabaseContext>(options => options
    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
    .AddInterceptors(new VietnamTimeInterceptor()));

// Add identity, for authentication and authorization.
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
    
    Func<RedirectContext<CookieAuthenticationOptions>,Task> authRedirectInterceptor;
    authRedirectInterceptor = (context) =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    

    options.Events.OnRedirectToLogin = authRedirectInterceptor;
    options.Events.OnRedirectToAccessDenied = authRedirectInterceptor;

    options.Events.OnRedirectToLogout = (context) =>
    {
        context.Response.StatusCode = StatusCodes.Status200OK;
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme);

// FluentValidation.
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
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ICertificateService, CertificateService>();
builder.Services.AddScoped<ISummaryItemService, SummaryItemService>();
builder.Services.AddScoped<ISummaryItemService, SummaryItemService>();
builder.Services.AddScoped<ICatalogItemService, CatalogItemService>();
builder.Services.AddScoped<ISliderItemService, SliderItemService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IEnquiryService, EnquiryService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ITrafficService, TrafficService>();
builder.Services.AddScoped<CssUniqueIdHelper>();

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
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
// app.UseMiddleware<TrafficMiddleware>();
app.UseMiddleware<UnderMaintainanceMiddleware>();
app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (AuthenticationException)
    {
        context.Response.Redirect("/SignIn");
    }
});
app.MapControllerRoute("default", pattern: "{controller=FrontPageHome}/{action=Index}");
app.Run();