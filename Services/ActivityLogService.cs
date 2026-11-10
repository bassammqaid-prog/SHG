using System.Security.Claims;
using Al_Shami_AISS_.Models;
using Al_Shami_AISS_.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Al_Shami_AISS_.Services;

/// <summary>
/// Central service for auditing business actions and user activity.
/// This service is the application-wide logging hub. Controllers and business services call it
/// so that every important action is recorded consistently without duplicating logic.
/// </summary>
public class ActivityLogService : IActivityLogService
{
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes the service with the repository and HTTP context accessor.
    /// The current request context is used to capture the actor and IP address automatically.
    /// </summary>
    public ActivityLogService(IActivityLogRepository activityLogRepository, IHttpContextAccessor httpContextAccessor)
    {
        _activityLogRepository = activityLogRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ActivityLog>> GetAllAsync()
    {
        return await _activityLogRepository.GetAllAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ActivityLog>> GetRecentAsync(int count = 10)
    {
        return await _activityLogRepository.GetRecentAsync(count);
    }

    /// <inheritdoc />
    public async Task<ActivityLog?> GetByIdAsync(int id)
    {
        return await _activityLogRepository.GetByIdAsync(id);
    }

    /// <inheritdoc />
    public async Task<ActivityLog> LogAsync(string action, string entityName, int? entityId, string description, string? userId = null, string? userEmail = null, string? ipAddress = null)
    {
        var currentUser = _httpContextAccessor.HttpContext?.User;
        var logEntry = new ActivityLog
        {
            UserId = userId ?? currentUser?.FindFirstValue(ClaimTypes.NameIdentifier),
            UserEmail = userEmail ?? currentUser?.FindFirstValue(ClaimTypes.Email) ?? currentUser?.Identity?.Name,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            Description = description,
            IpAddress = ipAddress ?? _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
            CreatedAt = DateTime.UtcNow
        };

        await _activityLogRepository.AddAsync(logEntry);
        await _activityLogRepository.SaveChangesAsync();

        return logEntry;
    }

    /// <inheritdoc />
    public async Task LogProductCreatedAsync(Product product)
    {
        await LogAsync(
            "Product Created",
            nameof(Product),
            product.Id,
            $"Product '{product.Name}' was created.");
    }

    /// <inheritdoc />
    public async Task LogProductUpdatedAsync(Product product)
    {
        await LogAsync(
            "Product Updated",
            nameof(Product),
            product.Id,
            $"Product '{product.Name}' was updated.");
    }

    /// <inheritdoc />
    public async Task LogProductDeletedAsync(Product product)
    {
        await LogAsync(
            "Product Deleted",
            nameof(Product),
            product.Id,
            $"Product '{product.Name}' was deleted.");
    }

    /// <inheritdoc />
    public async Task LogCategoryCreatedAsync(Category category)
    {
        await LogAsync(
            "Category Created",
            nameof(Category),
            category.Id,
            $"Category '{category.Name}' was created.");
    }

    /// <inheritdoc />
    public async Task LogCategoryUpdatedAsync(Category category)
    {
        await LogAsync(
            "Category Updated",
            nameof(Category),
            category.Id,
            $"Category '{category.Name}' was updated.");
    }

    /// <inheritdoc />
    public async Task LogCategoryDeletedAsync(Category category)
    {
        await LogAsync(
            "Category Deleted",
            nameof(Category),
            category.Id,
            $"Category '{category.Name}' was deleted.");
    }

    /// <inheritdoc />
    public async Task LogProjectCreatedAsync(Project project)
    {
        await LogAsync(
            "Project Created",
            nameof(Project),
            project.Id,
            $"Project '{project.Title}' was created.");
    }

    /// <inheritdoc />
    public async Task LogProjectUpdatedAsync(Project project)
    {
        await LogAsync(
            "Project Updated",
            nameof(Project),
            project.Id,
            $"Project '{project.Title}' was updated.");
    }

    /// <inheritdoc />
    public async Task LogProjectDeletedAsync(Project project)
    {
        await LogAsync(
            "Project Deleted",
            nameof(Project),
            project.Id,
            $"Project '{project.Title}' was deleted.");
    }

    /// <inheritdoc />
    public async Task LogLoginAsync(string userId, string? userEmail)
    {
        await LogAsync(
            "User Login",
            "User",
            null,
            $"User '{userEmail ?? userId}' logged in.",
            userId,
            userEmail);
    }

    /// <inheritdoc />
    public async Task LogLogoutAsync(string userId, string? userEmail)
    {
        await LogAsync(
            "User Logout",
            "User",
            null,
            $"User '{userEmail ?? userId}' logged out.",
            userId,
            userEmail);
    }
}
