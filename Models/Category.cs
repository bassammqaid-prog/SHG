namespace Al_Shami_ِAISS_.Models;

/// <summary>
/// Represents a product category used to organize catalog items.
/// Each category can contain many products, making it a common grouping for
/// inventory, pricing, and filtering features across the storefront.
/// </summary>
public class Category
{
    /// <summary>
    /// Unique identifier for the category.
    /// EF Core uses this as the primary key.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Business-friendly name shown to administrators and customers.
    /// Example: "Lighting", "HVAC", or "Electrical Accessories".
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional description that explains the purpose of the category.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indicates whether the category is currently available for use.
    /// Inactive categories can be hidden from public listings without deletion.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Date and time when the category was created.
    /// This is useful for auditing and reporting.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navigation property for all products assigned to this category.
    /// This enables the one-to-many relationship between Category and Product.
    /// </summary>
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
