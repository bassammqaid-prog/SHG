using Al_Shami_AISS_.Models;

namespace Al_Shami_AISS_.Services;

/// <summary>
/// Defines the activity logging contract for the application.
/// The interface keeps the service layer consistent and allows controllers and business services to log events without depending on a specific implementation.
/// </summary>
public interface IActivityLogService
{
    /// <summary>
    /// Retrieves all log entries.
    /// </summary>
    Task<IEnumerable<ActivityLog>> GetAllAsync();

    /// <summary>
    /// Retrieves the most recent log entries.
    /// </summary>
    Task<IEnumerable<ActivityLog>> GetRecentAsync(int count = 10);

    /// <summary>
    /// Retrieves a single log entry by identifier.
    /// </summary>
    Task<ActivityLog?> GetByIdAsync(int id);

    /// <summary>
    /// Logs a generic application action.
    /// </summary>
    Task<ActivityLog> LogAsync(string action, string entityName, int? entityId, string description, string? userId = null, string? userEmail = null, string? ipAddress = null);

    Task LogProductCreatedAsync(Product product);
    Task LogProductUpdatedAsync(Product product);
    Task LogProductDeletedAsync(Product product);

    Task LogCategoryCreatedAsync(Category category);
    Task LogCategoryUpdatedAsync(Category category);
    Task LogCategoryDeletedAsync(Category category);

    Task LogProjectCreatedAsync(Project project);
    Task LogProjectUpdatedAsync(Project project);
    Task LogProjectDeletedAsync(Project project);

    Task LogLoginAsync(string userId, string? userEmail);
    Task LogLogoutAsync(string userId, string? userEmail);
}
