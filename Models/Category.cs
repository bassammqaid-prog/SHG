namespace Al_Shami_AISS_.Models;

/// <summary>
/// Represents a product category used to organize catalog items.
/// Each category can contain many products, making it a common grouping for
/// inventory, pricing, and filtering features across the storefront.
/// SEO metadata is attached here so category pages can participate in search indexing,
/// canonical URLs, and friendly browsing URLs without creating a separate domain entity.
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

    /// <summary>
    /// SEO title displayed in browser tabs and search engine previews for this category page.
    /// </summary>
    public string? MetaTitle { get; set; }

    /// <summary>
    /// Search engine summary for the category landing page.
    /// </summary>
    public string? MetaDescription { get; set; }

    /// <summary>
    /// Keyword list used to enhance search visibility for the category.
    /// </summary>
    public string? MetaKeywords { get; set; }

    /// <summary>
    /// URL slug used to expose a readable and search-friendly category route.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Canonical URL used to prevent duplicate category content across multiple routes.
    /// </summary>
    public string? CanonicalUrl { get; set; }
}
