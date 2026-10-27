using Al_Shami_ِAISS_.Models;

namespace Al_Shami_ِAISS_.Services;

/// <summary>
/// Defines the email sending contract used throughout the application.
/// This abstraction allows the system to send admin notifications and user confirmations without hard-coding SMTP logic into controllers.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends a generic email message to a recipient.
    /// </summary>
    Task SendAsync(string to, string subject, string body, bool isHtml = true);

    /// <summary>
    /// Sends a notification email to the admin inbox whenever a contact message is submitted.
    /// </summary>
    Task SendContactNotificationAsync(ContactMessage message);

    /// <summary>
    /// Sends a confirmation email to the visitor after a contact form submission.
    /// </summary>
    Task SendContactConfirmationAsync(ContactMessage message);

    /// <summary>
    /// Sends a notification email to the admin inbox whenever a quote request is submitted.
    /// </summary>
    Task SendQuoteRequestNotificationAsync(QuoteRequest request);

    /// <summary>
    /// Sends a confirmation email to the requester after a quote form submission.
    /// </summary>
    Task SendQuoteRequestConfirmationAsync(QuoteRequest request);
}
