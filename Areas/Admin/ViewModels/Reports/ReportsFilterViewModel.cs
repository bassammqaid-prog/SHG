namespace Al_Shami_AISS_.Areas.Admin.ViewModels.Reports;

/// <summary>
/// Represents the filter criteria shared across report summaries and export actions.
/// These values allow administrators to narrow report results by date, category, project, user, or activity type.
/// </summary>
public class ReportsFilterViewModel
{
    /// <summary>
    /// gets or sets the start date for the report range.
    /// </summary>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// gets or sets the end date for the report range.
    /// </summary>
    public DateTime? DateTo { get; set; }

    /// <summary>
    /// gets or sets the category identifier used to filter catalog and sales reports.
    /// </summary>
    public int? CategoryId { get; set; }

    /// <summary>
    /// gets or sets the project identifier used to filter project-specific reporting.
    /// </summary>
    public int? ProjectId { get; set; }

    /// <summary>
    /// gets or sets the user identifier used to filter activity history and audit reports.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// gets or sets the activity type used to filter activity log reports.
    /// </summary>
    public string? ActivityType { get; set; }
}
