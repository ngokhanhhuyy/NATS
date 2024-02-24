using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

// Add signalR
builder.Services.AddSignalR();
// Add services to the container.
builder.Services.AddControllersWithViews();
string connectionString = builder.Configuration.GetConnectionString("MySQL");
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
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
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
ValidatorOptions.Global.LanguageManager.Enabled = true;
ValidatorOptions.Global.LanguageManager = new ValidatorLanguageManager {
    Culture = new CultureInfo("vi")
};
ValidatorOptions.Global.PropertyNameResolver = (_, b, _) => b.Name.First().ToString().ToLower() + b.Name[1..];

// Dependency injection
builder.Services.AddScoped<SignInManager<User>>();
builder.Services.AddScoped<RoleManager<Role>>();
builder.Services.AddScoped<DatabaseContext>();

var app = builder.Build();
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
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");
app.Run();
