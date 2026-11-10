namespace Al_Shami_AISS_.Models;

/// <summary>
/// Represents a completed or in-progress project showcased by the company.
/// These records are commonly used on portfolio pages and client-facing dashboards.
/// SEO metadata allows the project detail page to rank better in search engines and
/// provide rich previews in social networks and browsers.
/// </summary>
public class Project
{
    /// <summary>
    /// Primary key for the project record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name or title of the project presented to customers.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Optional summary of the work completed or the scope of the project.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Main image URL for the project portfolio entry.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Name of the client or organization that commissioned the project.
    /// </summary>
    public string? ClientName { get; set; }

    /// <summary>
    /// Physical or business location linked to the project.
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Date when the project reached completion.
    /// This value may be null while the project is still in progress.
    /// </summary>
    public DateTime? CompletionDate { get; set; }

    /// <summary>
    /// Date and time when the project record was created.
    /// This is used for reporting, sorting, and audit history.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indicates whether the project should be presented as a feature highlight.
    /// </summary>
    public bool IsFeatured { get; set; }

    /// <summary>
    /// Collection of supporting gallery images for this project.
    /// The project can include a cover image plus multiple additional images for the portfolio section.
    /// </summary>
    public ICollection<ProjectImage> ProjectImages { get; set; } = new List<ProjectImage>();

    /// <summary>
    /// SEO title used for search results and browser tabs on the project page.
    /// </summary>
    public string? MetaTitle { get; set; }

    /// <summary>
    /// Search engine description for the project detail page.
    /// </summary>
    public string? MetaDescription { get; set; }

    /// <summary>
    /// Search keywords used by the metadata layer to improve indexing.
    /// </summary>
    public string? MetaKeywords { get; set; }

    /// <summary>
    /// Friendly URL segment used to present a readable project path.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Canonical URL for the project page to avoid duplicate-content problems.
    /// </summary>
    public string? CanonicalUrl { get; set; }
}
