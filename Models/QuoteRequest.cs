namespace Al_Shami_ِAISS_.Models;

/// <summary>
/// Represents a quote request submitted by a potential customer.
/// These records capture the lead information and the message they want to send.
/// </summary>
public class QuoteRequest
{
    /// <summary>
    /// Primary key for the quote request.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Full name of the person submitting the request.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Optional company name for business inquiries.
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    /// Email address used for follow-up contact.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Optional phone number for direct contact.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Free-text description of their requirements or inquiry.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Date and time when the request was submitted.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
