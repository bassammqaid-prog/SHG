namespace Al_Shami_ِAISS_.Areas.Admin.ViewModels;

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
}
