using Al_Shami_AISS_.Models;

namespace Al_Shami_AISS_.Repositories.Interfaces;

/// <summary>
/// Defines the persistence contract for the application activity log.
/// Repositories isolate EF Core from the service layer and make the logging feature easier to test and maintain.
/// </summary>
public interface IActivityLogRepository
{
    /// <summary>
    /// Returns all activity log entries in descending order by time.
    /// </summary>
    Task<IEnumerable<ActivityLog>> GetAllAsync();

    /// <summary>
    /// Returns the most recent activity log entries for the notification center and dashboard widgets.
    /// </summary>
    /// <param name="count">The maximum number of entries to return.</param>
    Task<IEnumerable<ActivityLog>> GetRecentAsync(int count = 10);

    /// <summary>
    /// Retrieves a single activity log entry by id.
    /// </summary>
    Task<ActivityLog?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new entry to the activity log.
    /// </summary>
    Task AddAsync(ActivityLog activityLog);

    /// <summary>
    /// Persists pending log entries to the database.
    /// </summary>
    Task<int> SaveChangesAsync();
}
