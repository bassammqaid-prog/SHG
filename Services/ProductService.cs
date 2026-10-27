using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Repositories.Interfaces;

namespace Al_Shami_ِAISS_.Services;

/// <summary>
/// Business logic layer for product operations.
/// This service centralizes validation, rules, and orchestration while repositories remain focused on persistence.
/// </summary>
public class ProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IActivityLogService _activityLogService;

    /// <summary>
    /// Initializes the product service with a repository dependency.
    /// </summary>
    /// <param name="productRepository">The product repository.</param>
    /// <param name="activityLogService">The logging service used to track catalog changes.</param>
    public ProductService(IProductRepository productRepository, IActivityLogService activityLogService)
    {
        _productRepository = productRepository;
        _activityLogService = activityLogService;
    }

    /// <summary>
    /// Gets all products, including their category data.
    /// </summary>
    /// <returns>A collection of products.</returns>
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    /// <summary>
    /// Gets a single product by identifier.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    /// <returns>The product, or null if not found.</returns>
    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Creates a product and saves it to the database.
    /// This is where rules such as required fields or validation can live.
    /// </summary>
    /// <param name="product">The product to create.</param>
    /// <returns>The saved product.</returns>
    public async Task<Product> AddAsync(Product product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new InvalidOperationException("Product name is required.");
        }

        if (product.CategoryId <= 0)
        {
            throw new InvalidOperationException("A valid category is required for each product.");
        }

        if (product.Price < 0)
        {
            throw new InvalidOperationException("Product price cannot be negative.");
        }

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();

        await _activityLogService.LogProductCreatedAsync(product);

        return product;
    }

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="product">The updated product.</param>
    public async Task UpdateAsync(Product product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new InvalidOperationException("Product name is required.");
        }

        if (product.Price < 0)
        {
            throw new InvalidOperationException("Product price cannot be negative.");
        }

        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync();

        await _activityLogService.LogProductUpdatedAsync(product);
    }

    /// <summary>
    /// Deletes a product by identifier.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    public async Task DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
        {
            throw new KeyNotFoundException($"Product with id {id} was not found.");
        }

        _productRepository.Delete(product);
        await _productRepository.SaveChangesAsync();

        await _activityLogService.LogProductDeletedAsync(product);
    }
}
