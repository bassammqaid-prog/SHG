using Al_Shami_ِAISS_.Data;
using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Al_Shami_ِAISS_.Repositories.Implementations;

/// <summary>
/// EF Core repository responsible for quote request persistence.
/// This class encapsulates SQL Server access and leaves service logic focused on business rules.
/// </summary>
public class QuoteRequestRepository : IQuoteRequestRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes the repository with the application DbContext.
    /// </summary>
    public QuoteRequestRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<QuoteRequest>> GetAllAsync()
    {
        return await _context.QuoteRequests
            .AsNoTracking()
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<QuoteRequest?> GetByIdAsync(int id)
    {
        return await _context.QuoteRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    /// <inheritdoc />
    public async Task AddAsync(QuoteRequest quoteRequest)
    {
        await _context.QuoteRequests.AddAsync(quoteRequest);
    }

    /// <inheritdoc />
    public void Update(QuoteRequest quoteRequest)
    {
        _context.QuoteRequests.Update(quoteRequest);
    }

    /// <inheritdoc />
    public void Delete(QuoteRequest quoteRequest)
    {
        _context.QuoteRequests.Remove(quoteRequest);
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
