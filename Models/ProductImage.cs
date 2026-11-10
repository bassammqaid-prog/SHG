namespace Al_Shami_AISS_.Models;

/// <summary>
/// Represents an additional image associated with a product.
/// A product can have a main image and several supporting gallery images.
/// </summary>
public class ProductImage
{
    /// <summary>
    /// Primary key for the product image record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// URL or stored path of the image file.
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key to the owning product.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Navigation property back to the product.
    /// EF Core uses this to maintain the relationship when loading related data.
    /// </summary>
    public Product Product { get; set; } = null!;
}
