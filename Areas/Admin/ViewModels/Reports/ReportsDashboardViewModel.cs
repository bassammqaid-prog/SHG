using Microsoft.AspNetCore.Mvc.Rendering;

namespace Al_Shami_AISS_.Areas.Admin.ViewModels.Reports;

/// <summary>
/// Represents the view model used by the reporting dashboard.
/// This model combines the summary counters, filtering selections, and option lists needed to render
/// the admin reporting screens and export workflows in a clean, testable way.
/// </summary>
public class ReportsDashboardViewModel
{
    /// <summary>
    /// Gets or sets the date from which the report should start.
    /// </summary>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// Gets or sets the date at which the report should end.
    /// </summary>
    public DateTime? DateTo { get; set; }

    /// <summary>
    /// Gets or sets the selected category identifier used to filter the report data.
    /// </summary>
    public int? CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the selected project identifier used to filter project-based results.
    /// </summary>
    public int? ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the selected user identifier used to filter activity or reporting data.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the selected activity type used to filter the activity log report.
    /// </summary>
    public string? ActivityType { get; set; }

    /// <summary>
    /// Gets or sets the total number of products currently represented in the report view.
    /// </summary>
    public int TotalProducts { get; set; }

    /// <summary>
    /// Gets or sets the total number of projects in the system or current filtered result set.
    /// </summary>
    public int TotalProjects { get; set; }

    /// <summary>
    /// Gets or sets the total number of catalog categories.
    /// </summary>
    public int TotalCategories { get; set; }

    /// <summary>
    /// Gets or sets the total number of registered users.
    /// </summary>
    public int TotalUsers { get; set; }

    /// <summary>
    /// Gets or sets the total number of contact messages received.
    /// </summary>
    public int TotalContactMessages { get; set; }

    /// <summary>
    /// Gets or sets the total number of quote requests captured.
    /// </summary>
    public int TotalQuoteRequests { get; set; }

    /// <summary>
    /// Gets or sets the total number of activity log entries.
    /// </summary>
    public int TotalActivityLogs { get; set; }

    /// <summary>
    /// Gets or sets the list of category values used for filtering dropdowns.
    /// </summary>
    public List<SelectListItem> Categories { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of project values used for filtering dropdowns.
    /// </summary>
    public List<SelectListItem> Projects { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of users used for filter dropdowns.
    /// </summary>
    public List<SelectListItem> Users { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of supported activity types used by the activity report filters.
    /// </summary>
    public List<SelectListItem> ActivityTypes { get; set; } = new();

    /// <summary>
    /// Gets or sets the UTC date used when the user downloads or views generated reports.
    /// </summary>
    public DateTime ReportDateUtc { get; set; } = DateTime.UtcNow;
}
