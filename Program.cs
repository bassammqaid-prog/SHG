using Al_Shami_ِAISS_.Data;
using Al_Shami_ِAISS_.Repositories.Implementations;
using Al_Shami_ِAISS_.Repositories.Interfaces;
using Al_Shami_ِAISS_.Services;
using Microsoft.EntityFrameworkCore;

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

// Add MVC support to the application so controllers and views can be used.
builder.Services.AddControllersWithViews();

// The DbContext is registered with SQL Server so every repository can use the same database connection.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Repositories are registered as scoped services. A scoped lifetime means each HTTP request
// receives its own instance, which helps keep state isolated and predictable.
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
builder.Services.AddScoped<IQuoteRequestRepository, QuoteRequestRepository>();

// Services sit above repositories and contain business logic. They are also registered as scoped
// services so controllers can request them through dependency injection.
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductImageService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<ContactMessageService>();
builder.Services.AddScoped<QuoteRequestService>();

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
app.UseRouting();
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

app.Run();
