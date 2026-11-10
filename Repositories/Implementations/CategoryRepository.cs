using Al_Shami_AISS_.Data;
using Al_Shami_AISS_.Models;
using Al_Shami_AISS_.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Al_Shami_AISS_.Repositories.Implementations;

/// <summary>
/// Entity Framework implementation of <see cref="ICategoryRepository"/>.
/// This class is responsible only for database access to the Categories table.
/// It does not contain business logic; business logic belongs in the service layer.
/// </summary>
public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes the repository with the application DbContext.
    /// ASP.NET Core injects the DbContext instance through dependency injection.
    /// </summary>
    /// <param name="context">The EF Core database context.</param>
    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        // AsNoTracking() improves read performance for queries that do not need change tracking.
        return await _context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .AsNoTracking()
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <inheritdoc />
    public async Task<Category?> GetBySlugAsync(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return null;
        }

        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Slug == slug);
    }

    /// <inheritdoc />
    public async Task AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
    }

    /// <inheritdoc />
    public void Update(Category category)
    {
        _context.Categories.Update(category);
    }

    /// <inheritdoc />
    public void Delete(Category category)
    {
        _context.Categories.Remove(category);
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
