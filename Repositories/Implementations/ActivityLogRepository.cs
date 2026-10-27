using Al_Shami_ِAISS_.Data;
using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Al_Shami_ِAISS_.Repositories.Implementations;

/// <summary>
/// Entity Framework implementation of the activity log repository.
/// This class handles log storage without exposing EF Core details to the rest of the application.
/// </summary>
public class ActivityLogRepository : IActivityLogRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes the repository with the application database context.
    /// </summary>
    public ActivityLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ActivityLog>> GetAllAsync()
    {
        return await _context.ActivityLogs
            .AsNoTracking()
            .OrderByDescending(log => log.CreatedAt)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ActivityLog>> GetRecentAsync(int count = 10)
    {
        return await _context.ActivityLogs
            .AsNoTracking()
            .OrderByDescending(log => log.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<ActivityLog?> GetByIdAsync(int id)
    {
        return await _context.ActivityLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(log => log.Id == id);
    }

    /// <inheritdoc />
    public async Task AddAsync(ActivityLog activityLog)
    {
        await _context.ActivityLogs.AddAsync(activityLog);
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
