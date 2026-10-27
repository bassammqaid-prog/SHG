namespace Al_Shami_ِAISS_.Models;

/// <summary>
/// Represents a business activity event recorded by the application.
/// This entity is used for operational monitoring, auditing, and internal troubleshooting.
/// It captures who did what, when, and from which IP address so administrators can review changes safely.
/// </summary>
public class ActivityLog
{
    /// <summary>
    /// Primary key for the activity log record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The identity user identifier associated with the action.
    /// This value allows us to connect a log entry to a specific authenticated user.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Email address of the user who triggered the action.
    /// This helps administrators identify the actor without exposing additional internal data.
    /// </summary>
    public string? UserEmail { get; set; }

    /// <summary>
    /// A short description of the action, such as Product Created or User Login.
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// The database entity involved in the event, such as Product, Category, or Project.
    /// </summary>
    public string EntityName { get; set; } = string.Empty;

    /// <summary>
    /// The identifier of the affected entity when available.
    /// For example, the product ID being created or deleted.
    /// </summary>
    public int? EntityId { get; set; }

    /// <summary>
    /// Detailed explanation of the operation and its business impact.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Client IP address associated with the request.
    /// This is useful during investigations and for respecting audit requirements.
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// UTC date and time when the event occurred.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
