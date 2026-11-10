using Al_Shami_AISS_.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Al_Shami_AISS_.Data;

/// <summary>
/// The main EF Core database context for the application.
/// It extends <see cref="IdentityDbContext{TUser}"/> so ASP.NET Core Identity can store its tables alongside the business tables.
/// This keeps user data, roles, claims, and the application's domain data in the same SQL Server database.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// ASP.NET Core dependency injection supplies the SQL Server configuration and Identity options.
    /// </summary>
    /// <param name="options">Configuration options used by EF Core and ASP.NET Core Identity.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Categories table.
    /// </summary>
    public DbSet<Category> Categories { get; set; }

    /// <summary>
    /// Products table.
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// Product images table.
    /// </summary>
    public DbSet<ProductImage> ProductImages { get; set; }

    /// <summary>
    /// Projects table for portfolio entries.
    /// </summary>
    public DbSet<Project> Projects { get; set; }

    /// <summary>
    /// Project gallery images table.
    /// </summary>
    public DbSet<ProjectImage> ProjectImages { get; set; }

    /// <summary>
    /// Activity log table used for business auditing and operational monitoring.
    /// </summary>
    public DbSet<ActivityLog> ActivityLogs { get; set; }

    /// <summary>
    /// Quote requests table for leads.
    /// </summary>
    public DbSet<QuoteRequest> QuoteRequests { get; set; }

    /// <summary>
    /// Contact messages table for general inquiries.
    /// </summary>
    public DbSet<ContactMessage> ContactMessages { get; set; }

    /// <summary>
    /// Configures the model and relationships using Fluent API.
    /// This is the preferred place for relationship rules that are more explicit than
    /// simple attribute-based configuration, especially when we need to customize
    /// cascade behavior or constraints.
    /// </summary>
    /// <param name="modelBuilder">The EF Core model builder used to define schema rules.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // The Category -> Product relationship is a classic one-to-many relationship:
        // one category can have many products, but each product belongs to one category.
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Product -> ProductImage relationship:
        // one product can have many images, and every image belongs to one product.
        modelBuilder.Entity<Product>()
            .HasMany(p => p.ProductImages)
            .WithOne(pi => pi.Product)
            .HasForeignKey(pi => pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // The Project -> ProjectImage relationship is another one-to-many relationship:
        // one project can have many gallery images, and each gallery image belongs to one project.
        // We use cascade delete because the images are dependent on the project record and should disappear
        // automatically if the project is removed from the database.
        modelBuilder.Entity<Project>()
            .HasMany(p => p.ProjectImages)
            .WithOne(pi => pi.Project)
            .HasForeignKey(pi => pi.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // We intentionally restrict deleting a category when products still exist.
        // This protects data integrity and prevents orphaned products from being lost.
        // If a category is deleted, the system will block the delete and require the
        // products to be reassigned or removed first.

        // We use cascading delete for images because product images are dependent data.
        // If a product is deleted, its gallery images should also be deleted automatically.
        // This avoids orphaned image records in the database.

        // Identity tables are created by ASP.NET Core Identity and include these main sets:
        // AspNetUsers -> user accounts and profile data
        // AspNetRoles -> roles such as Admin and Manager
        // AspNetUserRoles -> assignments between users and roles
        // AspNetUserClaims -> user-specific claim data
        // AspNetRoleClaims -> role-specific claim data
        // AspNetUserLogins -> external login providers
        // AspNetUserTokens -> security tokens used for password resets, 2FA, and cookie-based flows

        // Configure table names and schema conventions for readability.
        modelBuilder.Entity<Category>().ToTable("Categories");
        modelBuilder.Entity<Product>().ToTable("Products");
        modelBuilder.Entity<ProductImage>().ToTable("ProductImages");
        modelBuilder.Entity<Project>().ToTable("Projects");
        modelBuilder.Entity<ProjectImage>().ToTable("ProjectImages");
        modelBuilder.Entity<ActivityLog>().ToTable("ActivityLogs");
        modelBuilder.Entity<QuoteRequest>().ToTable("QuoteRequests");
        modelBuilder.Entity<ContactMessage>().ToTable("ContactMessages");

        // Seed data ensures the application starts with realistic catalog information.
        // This is especially useful for demos, local development, and testing with a working dataset.
        SeedData.Configure(modelBuilder);

        // These additional configurations help keep the database predictable and easy to maintain.
        modelBuilder.Entity<Category>()
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<Product>()
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Currency values should be stored with a fixed precision to avoid rounding issues.
        // This keeps money values stable and predictable in SQL Server.
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Product>()
            .Property(p => p.ModelNumber)
            .HasMaxLength(100);

        modelBuilder.Entity<ProductImage>()
            .Property(pi => pi.ImageUrl)
            .IsRequired();

        modelBuilder.Entity<Project>()
            .Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<ProjectImage>()
            .Property(pi => pi.ImageUrl)
            .IsRequired();

        modelBuilder.Entity<ProjectImage>()
            .Property(pi => pi.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<ActivityLog>()
            .Property(log => log.Action)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<ActivityLog>()
            .Property(log => log.EntityName)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<ActivityLog>()
            .Property(log => log.Description)
            .HasMaxLength(1000);

        modelBuilder.Entity<QuoteRequest>()
            .Property(q => q.FullName)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<QuoteRequest>()
            .Property(q => q.Email)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<ContactMessage>()
            .Property(c => c.FullName)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<ContactMessage>()
            .Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(200);
    }
}
