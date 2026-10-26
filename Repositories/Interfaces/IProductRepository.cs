using Al_Shami_ِAISS_.Models;

namespace Al_Shami_ِAISS_.Repositories.Interfaces;

/// <summary>
/// Defines the data-access contract for product operations.
/// Repositories usually expose a stable contract so the service layer can work
/// with product data without depending on the concrete EF Core implementation.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Returns all products in the database.
    /// </summary>
    /// <returns>A collection of products.</returns>
    Task<IEnumerable<Product>> GetAllAsync();

    /// <summary>
    /// Returns a product by its identifier, if one exists.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    /// <returns>The product, or null if not found.</returns>
    Task<Product?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new product to the repository.
    /// </summary>
    /// <param name="product">The product to add.</param>
    Task AddAsync(Product product);

    /// <summary>
    /// Marks an existing product as modified.
    /// </summary>
    /// <param name="product">The updated product.</param>
    void Update(Product product);

    /// <summary>
    /// Deletes a product from the repository.
    /// </summary>
    /// <param name="product">The product to delete.</param>
    void Delete(Product product);

    /// <summary>
    /// Saves all pending changes to the underlying database.
    /// </summary>
    /// <returns>The number of state entries written.</returns>
    Task<int> SaveChangesAsync();
}
