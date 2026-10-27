namespace Al_Shami_ِAISS_.Models;

/// <summary>
/// Holds the SMTP configuration used by the application email service.
/// The values are loaded from appsettings.json so the email system remains configurable across environments.
/// </summary>
public class EmailSettings
{
    /// <summary>
    /// SMTP server hostname or IP address.
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// SMTP port used for outbound email delivery.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Email address used as the sender of system messages.
    /// </summary>
    public string Sender { get; set; } = string.Empty;

    /// <summary>
    /// Username for SMTP authentication when required by the mail server.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Password for SMTP authentication when required by the mail server.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
