using Al_Shami_AISS_.Models;
using Microsoft.EntityFrameworkCore;

namespace Al_Shami_AISS_.Data;

/// <summary>
/// Contains sample catalog data used during development and initial database setup.
/// Seed data helps junior developers understand how the system behaves with real data,
/// while also making demo and testing scenarios easier to work with.
/// </summary>
public static class SeedData
{
    /// <summary>
    /// Shared creation timestamp for seeded records so they are consistent and easy to audit.
    /// </summary>
    private static readonly DateTime SeedTimestamp = new(2025, 1, 15, 9, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Configures seed data for categories and products.
    /// The category IDs are fixed so product records can reference them reliably.
    /// </summary>
    /// <param name="modelBuilder">The EF Core model builder instance.</param>
    public static void Configure(ModelBuilder modelBuilder)
    {
        // Seed categories first because products depend on these records through CategoryId.
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Electrical",
                Description = "High-performance electrical solutions for industrial and commercial applications.",
                IsActive = true,
                CreatedAt = SeedTimestamp
            },
            new Category
            {
                Id = 2,
                Name = "Plumbing",
                Description = "Reliable water systems, fittings, and flow-control products.",
                IsActive = true,
                CreatedAt = SeedTimestamp
            },
            new Category
            {
                Id = 3,
                Name = "HVAC",
                Description = "Climate control units and energy-efficient air treatment products.",
                IsActive = true,
                CreatedAt = SeedTimestamp
            },
            new Category
            {
                Id = 4,
                Name = "Lighting",
                Description = "Commercial and residential lighting solutions with durable performance.",
                IsActive = true,
                CreatedAt = SeedTimestamp
            },
            new Category
            {
                Id = 5,
                Name = "Safety",
                Description = "Protection, fire response, and monitoring equipment for safe operations.",
                IsActive = true,
                CreatedAt = SeedTimestamp
            });

        // Seed products after categories so the foreign keys match the seeded categories.
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Smart Circuit Breaker",
                Description = "A digital breaker that supports overload protection and remote monitoring.",
                Price = 245.00m,
                ImageUrl = "/images/products/circuit-breaker.jpg",
                ModelNumber = "CB-240-A",
                Brand = "AISS Power",
                StockQuantity = 32,
                IsFeatured = true,
                CategoryId = 1,
                CreatedAt = SeedTimestamp
            },
            new Product
            {
                Id = 2,
                Name = "Industrial Pipe Fitting",
                Description = "Heavy-duty fitting built for durable plumbing installations and high flow pressure.",
                Price = 98.50m,
                ImageUrl = "/images/products/pipe-fitting.jpg",
                ModelNumber = "PF-118",
                Brand = "FlowCore",
                StockQuantity = 48,
                IsFeatured = false,
                CategoryId = 2,
                CreatedAt = SeedTimestamp
            },
            new Product
            {
                Id = 3,
                Name = "Commercial Air Conditioner",
                Description = "Energy-savings cooling unit designed for mid-size commercial spaces.",
                Price = 1850.00m,
                ImageUrl = "/images/products/ac-unit.jpg",
                ModelNumber = "HVAC-5500",
                Brand = "ClimaPro",
                StockQuantity = 12,
                IsFeatured = true,
                CategoryId = 3,
                CreatedAt = SeedTimestamp
            },
            new Product
            {
                Id = 4,
                Name = "LED Panel Light",
                Description = "Slim lighting panel for streamlined commercial interiors and office spaces.",
                Price = 135.00m,
                ImageUrl = "/images/products/led-panel.jpg",
                ModelNumber = "LP-40W",
                Brand = "LumaBright",
                StockQuantity = 64,
                IsFeatured = true,
                CategoryId = 4,
                CreatedAt = SeedTimestamp
            },
            new Product
            {
                Id = 5,
                Name = "Fire Alarm Sensor",
                Description = "Early-warning smoke and heat detection device for safety monitoring systems.",
                Price = 86.00m,
                ImageUrl = "/images/products/fire-sensor.jpg",
                ModelNumber = "FA-88",
                Brand = "GuardSafe",
                StockQuantity = 90,
                IsFeatured = false,
                CategoryId = 5,
                CreatedAt = SeedTimestamp
            },
            new Product
            {
                Id = 6,
                Name = "Solar Power Inverter",
                Description = "Efficient power conversion unit for renewable energy applications.",
                Price = 640.00m,
                ImageUrl = "/images/products/solar-inverter.jpg",
                ModelNumber = "SOL-3K",
                Brand = "EcoVolt",
                StockQuantity = 20,
                IsFeatured = true,
                CategoryId = 1,
                CreatedAt = SeedTimestamp
            },
            new Product
            {
                Id = 7,
                Name = "Water Pump Unit",
                Description = "High-capacity pump designed for consistent pressure in commercial plumbing systems.",
                Price = 420.00m,
                ImageUrl = "/images/products/water-pump.jpg",
                ModelNumber = "WP-220",
                Brand = "HydroFlow",
                StockQuantity = 18,
                IsFeatured = false,
                CategoryId = 2,
                CreatedAt = SeedTimestamp
            });
    }
}
