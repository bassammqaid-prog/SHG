using Al_Shami_AISS_.Areas.Admin.ViewModels;
using Al_Shami_AISS_.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Al_Shami_AISS_.Areas.Admin.Controllers;

/// <summary>
/// Displays the application audit log for administrators.
/// The controller is protected to ensure only authorized users can review sensitive activity history.
/// </summary>
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ActivityLogsController : Controller
{
    private readonly IActivityLogService _activityLogService;

    /// <summary>
    /// Initializes the controller with the activity log service.
    /// </summary>
    public ActivityLogsController(IActivityLogService activityLogService)
    {
        _activityLogService = activityLogService;
    }

    /// <summary>
    /// Lists all activity entries in the system.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var activityLogs = await _activityLogService.GetAllAsync();

        var viewModel = activityLogs
            .Select(log => new ActivityLogListItemViewModel
            {
                Id = log.Id,
                UserDisplayName = string.IsNullOrWhiteSpace(log.UserEmail) ? (string.IsNullOrWhiteSpace(log.UserId) ? "System" : log.UserId) : log.UserEmail,
                Action = log.Action,
                EntityName = log.EntityName,
                Description = log.Description,
                IpAddress = log.IpAddress,
                CreatedAt = log.CreatedAt
            })
            .ToList();

        return View(viewModel);
    }

    /// <summary>
    /// Displays the full details for a selected activity log entry.
    /// </summary>
    public async Task<IActionResult> Details(int id)
    {
        var activityLog = await _activityLogService.GetByIdAsync(id);
        if (activityLog is null)
        {
            return NotFound();
        }

        var viewModel = new ActivityLogDetailViewModel
        {
            Id = activityLog.Id,
            UserDisplayName = string.IsNullOrWhiteSpace(activityLog.UserEmail) ? (string.IsNullOrWhiteSpace(activityLog.UserId) ? "System" : activityLog.UserId) : activityLog.UserEmail,
            Action = activityLog.Action,
            EntityName = activityLog.EntityName,
            EntityId = activityLog.EntityId,
            Description = activityLog.Description,
            IpAddress = activityLog.IpAddress,
            CreatedAt = activityLog.CreatedAt
        };

        return View(viewModel);
    }
}
