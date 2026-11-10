using Al_Shami_AISS_.Models;

namespace Al_Shami_AISS_.Repositories.Interfaces;

/// <summary>
/// Defines the repository contract for quote request persistence.
/// Service classes call this interface to remain independent from EF Core classes.
/// </summary>
public interface IQuoteRequestRepository
{
    /// <summary>
    /// Retrieves all quote requests.
    /// </summary>
    Task<IEnumerable<QuoteRequest>> GetAllAsync();

    /// <summary>
    /// Retrieves a quote request by identifier.
    /// </summary>
    Task<QuoteRequest?> GetByIdAsync(int id);

    /// <summary>
    /// Inserts a new quote request.
    /// </summary>
    Task AddAsync(QuoteRequest quoteRequest);

    /// <summary>
    /// Updates an existing quote request.
    /// </summary>
    void Update(QuoteRequest quoteRequest);

    /// <summary>
    /// Deletes a quote request.
    /// </summary>
    void Delete(QuoteRequest quoteRequest);

    /// <summary>
    /// Saves changes to SQL Server.
    /// </summary>
    Task<int> SaveChangesAsync();
}
