namespace Al_Shami_ِAISS_.Models;

/// <summary>
/// Represents a catalog product sold or promoted by the company.
/// A product belongs to a category and can have multiple gallery images.
/// </summary>
public class Product
{
    /// <summary>
    /// Primary key for the product record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Product name as it appears in the catalog or sales documents.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional detailed description of the item.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Selling price of the product.
    /// Using decimal keeps currency values precise.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Main image URL used for the product card or details page.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Manufacturer or model identifier used for internal tracking.
    /// </summary>
    public string? ModelNumber { get; set; }

    /// <summary>
    /// Brand or manufacturer name associated with the product.
    /// </summary>
    public string? Brand { get; set; }

    /// <summary>
    /// Quantity available in inventory.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Marks the product as a highlighted item in promotional sections.
    /// </summary>
    public bool IsFeatured { get; set; }

    /// <summary>
    /// Foreign key to the parent category.
    /// This allows each product to belong to exactly one category.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Navigation property pointing to the assigned category.
    /// EF Core resolves the relationship using this property and CategoryId.
    /// </summary>
    public Category Category { get; set; } = null!;

    /// <summary>
    /// Collection of additional product photos.
    /// One product can have many images, which is modeled with a one-to-many relationship.
    /// </summary>
    public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    /// <summary>
    /// Date and time when the product record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
