using System.Globalization;
using Al_Shami_ِAISS_.Data;
using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Repositories.Implementations;
using Al_Shami_ِAISS_.Repositories.Interfaces;
using Al_Shami_ِAISS_.Resources;
using Al_Shami_ِAISS_.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

// ASP.NET Core reads configuration values from appsettings.json and environment-specific files.
// This keeps sensitive or environment-specific settings out of code and makes deployment easier.

var builder = WebApplication.CreateBuilder(args);

// The application reads its database configuration from appsettings.json.
// This keeps the connection string out of source code and allows different values for
// development, staging, and production environments.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("The 'DefaultConnection' connection string is missing. Configure it in appsettings.json.");
}

// ASP.NET Core localization is configured before the MVC pipeline so the application can resolve
// translated strings and validation messages based on the current culture.
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Add MVC support to the application so controllers and views can be used.
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResource));
    });
builder.Services.AddHttpContextAccessor();

var supportedCultures = new[]
{
    new CultureInfo("ar-YE"),
    new CultureInfo("en-US")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("ar-YE");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.RequestCultureProviders = new IRequestCultureProvider[]
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider(),
        new AcceptLanguageHeaderRequestCultureProvider()
    };
});

// The DbContext is registered with SQL Server so every repository can use the same database connection.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// ASP.NET Core Identity adds the user, role, claim, and login stores required for authentication and authorization.
// This is the security layer used to protect admin pages and staff-only features.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Cookie-based authentication keeps the user signed in across requests by storing a secure encrypted cookie.
// The configured cookie settings help protect the app from unauthorized access and support the remember-me feature.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
    options.SlidingExpiration = true;
});

// Repositories are registered as scoped services. A scoped lifetime means each HTTP request
// receives its own instance, which helps keep state isolated and predictable.
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IProjectImageRepository, ProjectImageRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
builder.Services.AddScoped<IQuoteRequestRepository, QuoteRequestRepository>();
builder.Services.AddScoped<IActivityLogRepository, ActivityLogRepository>();

// Services sit above repositories and contain business logic. They are also registered as scoped
// services so controllers can request them through dependency injection.
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductImageService>();
builder.Services.AddScoped<ProjectImageService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<ContactMessageService>();
builder.Services.AddScoped<QuoteRequestService>();
builder.Services.AddScoped<ReportingService>();
builder.Services.AddScoped<IActivityLogService, ActivityLogService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IdentitySeedService>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

var app = builder.Build();

// Ensure the database exists and that any pending migrations are applied before the app starts serving requests.
// This also makes the seeded catalog data available immediately after startup.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
app.UseRouting();

// Authentication must be invoked before authorization so the application can determine who the visitor is.
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var identitySeedService = scope.ServiceProvider.GetRequiredService<IdentitySeedService>();
    await identitySeedService.InitializeAsync();
}

app.Run();
