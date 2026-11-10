using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Al_Shami_AISS_.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategoriesAndProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "High-performance electrical solutions for industrial and commercial applications.", true, "Electrical" },
                    { 2, new DateTime(2025, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Reliable water systems, fittings, and flow-control products.", true, "Plumbing" },
                    { 3, new DateTime(2025, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Climate control units and energy-efficient air treatment products.", true, "HVAC" },
                    { 4, new DateTime(2025, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Commercial and residential lighting solutions with durable performance.", true, "Lighting" },
                    { 5, new DateTime(2025, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Protection, fire response, and monitoring equipment for safe operations.", true, "Safety" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsFeatured", "ModelNumber", "Name", "Price", "StockQuantity" },
                values: new object[,]
                {
                    { 1, "AISS Power", 1, new DateTime(2025, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "A digital breaker that supports overload protection and remote monitoring.", "/images/products/circuit-breaker.jpg", true, "CB-240-A", "Smart Circuit Breaker", 245.00m, 32 },
                    { 2, "FlowCore", 2, new DateTime(2025, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Heavy-duty fitting built for durable plumbing installations and high flow pressure.", "/images/products/pipe-fitting.jpg", false, "PF-118", "Industrial Pipe Fitting", 98.50m, 48 },
                    { 3, "ClimaPro", 3, new DateTime(2025, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Energy-savings cooling unit designed for mid-size commercial spaces.", "/images/products/ac-unit.jpg", true, "HVAC-5500", "Commercial Air Conditioner", 1850.00m, 12 },
                    { 4, "LumaBright", 4, new DateTime(2025, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Slim lighting panel for streamlined commercial interiors and office spaces.", "/images/products/led-panel.jpg", true, "LP-40W", "LED Panel Light", 135.00m, 64 },
                    { 5, "GuardSafe", 5, new DateTime(2025, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Early-warning smoke and heat detection device for safety monitoring systems.", "/images/products/fire-sensor.jpg", false, "FA-88", "Fire Alarm Sensor", 86.00m, 90 },
                    { 6, "EcoVolt", 1, new DateTime(2025, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "Efficient power conversion unit for renewable energy applications.", "/images/products/solar-inverter.jpg", true, "SOL-3K", "Solar Power Inverter", 640.00m, 20 },
                    { 7, "HydroFlow", 2, new DateTime(2025, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "High-capacity pump designed for consistent pressure in commercial plumbing systems.", "/images/products/water-pump.jpg", false, "WP-220", "Water Pump Unit", 420.00m, 18 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
