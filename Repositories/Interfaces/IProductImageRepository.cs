using Al_Shami_AISS_.Models;

namespace Al_Shami_AISS_.Repositories.Interfaces;

/// <summary>
/// Defines the persistence contract for product image records.
/// This separates database access from business logic and keeps the service layer independent from EF Core implementation details.
/// </summary>
public interface IProductImageRepository
{
    /// <summary>
    /// Retrieves all images belonging to a specific product.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <returns>A collection of related product images.</returns>
    Task<IEnumerable<ProductImage>> GetByProductIdAsync(int productId);

    /// <summary>
    /// Retrieves a single product image by its database identifier.
    /// </summary>
    /// <param name="id">The image identifier.</param>
    /// <returns>The matching product image, or null if it is not found.</returns>
    Task<ProductImage?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new image record to the database.
    /// </summary>
    /// <param name="productImage">The image to add.</param>
    Task AddAsync(ProductImage productImage);

    /// <summary>
    /// Marks an image record for deletion.
    /// </summary>
    /// <param name="productImage">The image to delete.</param>
    void Delete(ProductImage productImage);

    /// <summary>
    /// Persists all pending image operations to the database.
    /// </summary>
    /// <returns>The number of rows affected.</returns>
    Task<int> SaveChangesAsync();
}
