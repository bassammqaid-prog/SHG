using Al_Shami_ِAISS_.Models;
using Microsoft.EntityFrameworkCore;

namespace Al_Shami_ِAISS_.Data;

/// <summary>
/// The main EF Core database context used by the application.
/// It is the central gateway to SQL Server and encapsulates the entity model.
/// Every repository eventually works through this class, which keeps the application
/// consistent with the repository and service architecture.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// The ASP.NET Core dependency injection container injects the DbContext options.
    /// </summary>
    /// <param name="options">Configuration options used by EF Core, such as the SQL Server connection.</param>
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

        // We intentionally restrict deleting a category when products still exist.
        // This protects data integrity and prevents orphaned products from being lost.
        // If a category is deleted, the system will block the delete and require the
        // products to be reassigned or removed first.

        // We use cascading delete for images because product images are dependent data.
        // If a product is deleted, its gallery images should also be deleted automatically.
        // This avoids orphaned image records in the database.

        // Configure table names and schema conventions for readability.
        modelBuilder.Entity<Category>().ToTable("Categories");
        modelBuilder.Entity<Product>().ToTable("Products");
        modelBuilder.Entity<ProductImage>().ToTable("ProductImages");
        modelBuilder.Entity<Project>().ToTable("Projects");
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
