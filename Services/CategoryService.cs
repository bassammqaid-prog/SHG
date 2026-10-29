using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Repositories.Interfaces;

namespace Al_Shami_ِAISS_.Services;

/// <summary>
/// Business logic layer for category operations.
/// The service applies rules and coordinates repository calls, while controllers stay focused on HTTP concerns.
/// This makes the application easier to test and keeps business logic centralized.
/// </summary>
public class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IActivityLogService _activityLogService;

    /// <summary>
    /// Initializes the category service with its repository dependency.
    /// Dependency injection supplies the repository and avoids hard-coded database access.
    /// </summary>
    /// <param name="categoryRepository">The category repository.</param>
    /// <param name="activityLogService">The service responsible for recording category-related audit entries.</param>
    public CategoryService(ICategoryRepository categoryRepository, IActivityLogService activityLogService)
    {
        _categoryRepository = categoryRepository;
        _activityLogService = activityLogService;
    }

    /// <summary>
    /// Retrieves all categories from the data layer.
    /// </summary>
    /// <returns>A collection of categories.</returns>
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _categoryRepository.GetAllAsync();
    }

    /// <summary>
    /// Retrieves a single category by identifier.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    /// <returns>The category if found; otherwise null.</returns>
    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Retrieves a category by its SEO-friendly slug.
    /// </summary>
    /// <param name="slug">The public URL slug.</param>
    /// <returns>The matching category, or null if no item is found.</returns>
    public async Task<Category?> GetBySlugAsync(string slug)
    {
        return await _categoryRepository.GetBySlugAsync(slug);
    }

    /// <summary>
    /// Creates a new category and saves it to the database.
    /// Business rules can be expanded here without changing controllers or repositories.
    /// </summary>
    /// <param name="category">The category to create.</param>
    /// <returns>The created category.</returns>
    public async Task<Category> AddAsync(Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }

        if (string.IsNullOrWhiteSpace(category.Name))
        {
            throw new InvalidOperationException("Category name is required.");
        }

        category.Slug = await SeoSlugService.EnsureUniqueSlugAsync(category.Name, async slug => await _categoryRepository.GetBySlugAsync(slug) != null);
        category.CanonicalUrl = $"/categories/{category.Slug}";

        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();

        await _activityLogService.LogCategoryCreatedAsync(category);

        return category;
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="category">The category with updated values.</param>
    public async Task UpdateAsync(Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }

        if (string.IsNullOrWhiteSpace(category.Name))
        {
            throw new InvalidOperationException("Category name is required.");
        }

        category.Slug = await SeoSlugService.EnsureUniqueSlugAsync(category.Name, async slug => await _categoryRepository.GetBySlugAsync(slug) != null, category.Slug);
        category.CanonicalUrl = $"/categories/{category.Slug}";

        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync();

        await _activityLogService.LogCategoryUpdatedAsync(category);
    }

    /// <summary>
    /// Deletes a category after validating that the item exists.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    public async Task DeleteAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            throw new KeyNotFoundException($"Category with id {id} was not found.");
        }

        _categoryRepository.Delete(category);
        await _categoryRepository.SaveChangesAsync();

        await _activityLogService.LogCategoryDeletedAsync(category);
    }
}
