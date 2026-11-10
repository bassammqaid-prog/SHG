namespace Al_Shami_AISS_.Areas.Admin.ViewModels;

/// <summary>
/// Represents the summary data displayed on the admin dashboard.
/// This ViewModel keeps the UI model separate from the database entities and is easy to bind in Razor.
/// </summary>
public class DashboardViewModel
{
    /// <summary>
    /// Total number of active categories in the catalog.
    /// </summary>
    public int TotalCategories { get; set; }

    /// <summary>
    /// Total number of products in the catalog.
    /// </summary>
    public int TotalProducts { get; set; }

    /// <summary>
    /// Total number of portfolio projects.
    /// </summary>
    public int TotalProjects { get; set; }

    /// <summary>
    /// Total number of contact form submissions received.
    /// </summary>
    public int TotalContactMessages { get; set; }

    /// <summary>
    /// Total number of quote request submissions.
    /// </summary>
    public int TotalQuoteRequests { get; set; }

    /// <summary>
    /// Total number of registered users in the application.
    /// </summary>
    public int TotalUsers { get; set; }

    /// <summary>
    /// Total number of activity log entries recorded in the system.
    /// </summary>
    public int TotalActivityLogs { get; set; }

    /// <summary>
    /// Recent business actions recorded for the admin notification center.
    /// </summary>
    public List<RecentNotificationViewModel> RecentActivityLogs { get; set; } = new();

    /// <summary>
    /// Recent contact submissions awaiting review.
    /// </summary>
    public List<RecentNotificationViewModel> RecentContactMessages { get; set; } = new();

    /// <summary>
    /// Recent quote requests awaiting review.
    /// </summary>
    public List<RecentNotificationViewModel> RecentQuoteRequests { get; set; } = new();
}
