namespace Al_Shami_ِAISS_.Models;

/// <summary>
/// Represents a general contact form message sent by a site visitor.
/// This model supports customer support and general business communication.
/// </summary>
public class ContactMessage
{
    /// <summary>
    /// Primary key for the contact message.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the person who sent the message.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Email address for follow-up communication.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Optional phone number for quick contact.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Subject line or category of the message.
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Body of the contact message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Date and time when the message was submitted.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
