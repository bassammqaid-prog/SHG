using Al_Shami_AISS_.Models;
using Al_Shami_AISS_.Repositories.Interfaces;

namespace Al_Shami_AISS_.Services;

/// <summary>
/// Business service for quote request management.
/// This layer validates lead submissions and delegates persistence responsibilities to the repository.
/// </summary>
public class QuoteRequestService
{
    private readonly IQuoteRequestRepository _quoteRequestRepository;

    /// <summary>
    /// Initializes the service with a repository dependency.
    /// </summary>
    public QuoteRequestService(IQuoteRequestRepository quoteRequestRepository)
    {
        _quoteRequestRepository = quoteRequestRepository;
    }

    /// <summary>
    /// Retrieves all quote requests.
    /// </summary>
    public async Task<IEnumerable<QuoteRequest>> GetAllAsync()
    {
        return await _quoteRequestRepository.GetAllAsync();
    }

    /// <summary>
    /// Retrieves a single quote request by identifier.
    /// </summary>
    public async Task<QuoteRequest?> GetByIdAsync(int id)
    {
        return await _quoteRequestRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Creates a new quote request after validation.
    /// </summary>
    public async Task<QuoteRequest> AddAsync(QuoteRequest quoteRequest)
    {
        if (quoteRequest == null)
        {
            throw new ArgumentNullException(nameof(quoteRequest));
        }

        if (string.IsNullOrWhiteSpace(quoteRequest.FullName))
        {
            throw new InvalidOperationException("Full name is required.");
        }

        if (string.IsNullOrWhiteSpace(quoteRequest.Email))
        {
            throw new InvalidOperationException("Email is required.");
        }

        if (string.IsNullOrWhiteSpace(quoteRequest.Message))
        {
            throw new InvalidOperationException("Message is required.");
        }

        await _quoteRequestRepository.AddAsync(quoteRequest);
        await _quoteRequestRepository.SaveChangesAsync();

        return quoteRequest;
    }

    /// <summary>
    /// Updates an existing quote request.
    /// </summary>
    public async Task UpdateAsync(QuoteRequest quoteRequest)
    {
        if (quoteRequest == null)
        {
            throw new ArgumentNullException(nameof(quoteRequest));
        }

        if (string.IsNullOrWhiteSpace(quoteRequest.FullName))
        {
            throw new InvalidOperationException("Full name is required.");
        }

        _quoteRequestRepository.Update(quoteRequest);
        await _quoteRequestRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a quote request by identifier.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var quoteRequest = await _quoteRequestRepository.GetByIdAsync(id);

        if (quoteRequest == null)
        {
            throw new KeyNotFoundException($"Quote request with id {id} was not found.");
        }

        _quoteRequestRepository.Delete(quoteRequest);
        await _quoteRequestRepository.SaveChangesAsync();
    }
}
