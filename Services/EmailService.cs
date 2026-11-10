using System.Net;
using System.Net.Mail;
using Al_Shami_AISS_.Models;
using Microsoft.Extensions.Options;

namespace Al_Shami_AISS_.Services;

/// <summary>
/// Sends transactional email messages through SMTP.
/// This service centralizes the mailing logic so controllers remain focused on business flow and validation.
/// </summary>
public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    /// <summary>
    /// Initializes the email service with SMTP configuration loaded from appsettings.json.
    /// </summary>
    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    /// <inheritdoc />
    public async Task SendAsync(string to, string subject, string body, bool isHtml = true)
    {
        if (string.IsNullOrWhiteSpace(_emailSettings.Host) || string.IsNullOrWhiteSpace(_emailSettings.Sender))
        {
            // SMTP configuration is optional at design time, but the application should not fail if the environment is not configured.
            // In production, administrators should provide host credentials in appsettings.json or environment variables.
            return;
        }

        using var message = new MailMessage
        {
            From = new MailAddress(_emailSettings.Sender, "AISS System"),
            Subject = subject,
            Body = body,
            IsBodyHtml = isHtml
        };

        message.To.Add(to);

        using var smtpClient = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
        {
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password)
        };

        try
        {
            await smtpClient.SendMailAsync(message);
        }
        catch (SmtpException ex)
        {
            // We never expose stack traces to users. Instead, we fail safely and keep the request flow intact.
            Console.WriteLine($"Email sending failed: {ex.Message}");
        }
    }

    /// <inheritdoc />
    public async Task SendContactNotificationAsync(ContactMessage message)
    {
        var subject = $"New Contact Message: {message.Subject}";
        var body = $@"
            <html>
                <body>
                    <h2>New Customer Inquiry</h2>
                    <p><strong>Name:</strong> {message.FullName}</p>
                    <p><strong>Email:</strong> {message.Email}</p>
                    <p><strong>Phone:</strong> {message.Phone ?? "Not provided"}</p>
                    <p><strong>Subject:</strong> {message.Subject}</p>
                    <p><strong>Message:</strong></p>
                    <p>{message.Message}</p>
                </body>
            </html>";

        await SendAsync(_emailSettings.Sender, subject, body, true);
    }

    /// <inheritdoc />
    public async Task SendContactConfirmationAsync(ContactMessage message)
    {
        var subject = "We received your message";
        var body = $@"
            <html>
                <body>
                    <h2>Thank you for contacting AISS</h2>
                    <p>Dear {message.FullName},</p>
                    <p>We have received your message and will get back to you shortly.</p>
                    <p><strong>Subject:</strong> {message.Subject}</p>
                    <p>Best regards,<br />AISS Team</p>
                </body>
            </html>";

        await SendAsync(message.Email, subject, body, true);
    }

    /// <inheritdoc />
    public async Task SendQuoteRequestNotificationAsync(QuoteRequest request)
    {
        var subject = $"New Quote Request: {request.CompanyName ?? request.FullName}";
        var body = $@"
            <html>
                <body>
                    <h2>New Quote Request</h2>
                    <p><strong>Full Name:</strong> {request.FullName}</p>
                    <p><strong>Company:</strong> {request.CompanyName ?? "Not provided"}</p>
                    <p><strong>Email:</strong> {request.Email}</p>
                    <p><strong>Phone:</strong> {request.Phone ?? "Not provided"}</p>
                    <p><strong>Message:</strong></p>
                    <p>{request.Message}</p>
                </body>
            </html>";

        await SendAsync(_emailSettings.Sender, subject, body, true);
    }

    /// <inheritdoc />
    public async Task SendQuoteRequestConfirmationAsync(QuoteRequest request)
    {
        var subject = "Your quote request has been received";
        var body = $@"
            <html>
                <body>
                    <h2>Quote Request Submitted</h2>
                    <p>Dear {request.FullName},</p>
                    <p>Thank you for requesting a quote from AISS. Our team will review your requirements and contact you soon.</p>
                    <p><strong>Company:</strong> {request.CompanyName ?? "Not provided"}</p>
                    <p>Best regards,<br />AISS Team</p>
                </body>
            </html>";

        await SendAsync(request.Email, subject, body, true);
    }
}
