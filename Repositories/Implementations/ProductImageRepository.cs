using Al_Shami_AISS_.Data;
using Al_Shami_AISS_.Models;
using Al_Shami_AISS_.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Al_Shami_AISS_.Repositories.Implementations;

/// <summary>
/// Entity Framework implementation for product image storage.
/// The repository handles all database actions while the service layer focuses on business rules.
/// </summary>
public class ProductImageRepository : IProductImageRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes the repository with the application database context.
    /// </summary>
    /// <param name="context">The EF Core database context.</param>
    public ProductImageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProductImage>> GetByProductIdAsync(int productId)
    {
        return await _context.ProductImages
            .AsNoTracking()
            .Where(pi => pi.ProductId == productId)
            .OrderBy(pi => pi.Id)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<ProductImage?> GetByIdAsync(int id)
    {
        return await _context.ProductImages
            .AsNoTracking()
            .FirstOrDefaultAsync(pi => pi.Id == id);
    }

    /// <inheritdoc />
    public async Task AddAsync(ProductImage productImage)
    {
        await _context.ProductImages.AddAsync(productImage);
    }

    /// <inheritdoc />
    public void Delete(ProductImage productImage)
    {
        _context.ProductImages.Remove(productImage);
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
