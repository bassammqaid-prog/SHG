using Al_Shami_ِAISS_.Data;
using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Al_Shami_ِAISS_.Repositories.Implementations;

/// <summary>
/// Entity Framework implementation of <see cref="IProductRepository"/>.
/// It handles database operations for products and their related data.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a product repository with the application DbContext.
    /// </summary>
    /// <param name="context">The EF Core database context.</param>
    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <inheritdoc />
    public async Task<Product?> GetBySlugAsync(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return null;
        }

        return await _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }

    /// <inheritdoc />
    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    /// <inheritdoc />
    public void Update(Product product)
    {
        _context.Products.Update(product);
    }

    /// <inheritdoc />
    public void Delete(Product product)
    {
        _context.Products.Remove(product);
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
