namespace Al_Shami_ِAISS_.Areas.Admin.ViewModels;

/// <summary>
/// Represents a single line item in the activity log list.
/// This view model keeps the UI model separate from the <see cref="Al_Shami_ِAISS_.Models.ActivityLog"/> entity.
/// </summary>
public class ActivityLogListItemViewModel
{
    public int Id { get; set; }
    public string UserDisplayName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Represents the full details of an activity log record.
/// </summary>
public class ActivityLogDetailViewModel
{
    public int Id { get; set; }
    public string UserDisplayName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public int? EntityId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? IpAddress { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Represents a recent activity or submission item shown on the dashboard notification center.
/// </summary>
public class RecentNotificationViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
