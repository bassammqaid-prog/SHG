using Al_Shami_AISS_.Models;

namespace Al_Shami_AISS_.Repositories.Interfaces;

/// <summary>
/// Defines the data-access contract for category operations.
/// The repository interface isolates the service layer from EF Core details,
/// which allows business logic to depend on abstractions rather than concrete data access code.
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Returns all categories in the database.
    /// </summary>
    /// <returns>A collection of categories.</returns>
    Task<IEnumerable<Category>> GetAllAsync();

    /// <summary>
    /// Returns a category by its identifier, if one exists.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    /// <returns>The category, or null if not found.</returns>
    Task<Category?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new category to the repository.
    /// </summary>
    /// <param name="category">The category to add.</param>
    Task AddAsync(Category category);

    /// <summary>
    /// Marks an existing category as modified.
    /// </summary>
    /// <param name="category">The updated category.</param>
    void Update(Category category);

    /// <summary>
    /// Deletes a category from the repository.
    /// </summary>
    /// <param name="category">The category to delete.</param>
    void Delete(Category category);

    /// <summary>
    /// Retrieves a category using its URL slug for SEO-friendly routing.
    /// </summary>
    /// <param name="slug">The SEO-friendly slug to find.</param>
    /// <returns>The matching category or null.</returns>
    Task<Category?> GetBySlugAsync(string slug);

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync();
}
