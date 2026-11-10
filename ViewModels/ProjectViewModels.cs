namespace Al_Shami_AISS_.ViewModels;

/// <summary>
/// Represents the public project listing page.
/// This allows the site to show a clean set of project data without exposing EF entities to the view layer.
/// </summary>
public class ProjectListViewModel
{
    public List<ProjectCardViewModel> Projects { get; set; } = new();
}

/// <summary>
/// Represents a single project card on the public project list.
/// </summary>
public class ProjectCardViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ClientName { get; set; }
    public string? Location { get; set; }
    public string? ImageUrl { get; set; }
}

/// <summary>
/// Represents the project detail page for public visitors.
/// </summary>
public class ProjectDetailsViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? ClientName { get; set; }
    public string? Location { get; set; }
    public DateTime? CompletionDate { get; set; }
    public bool IsFeatured { get; set; }
    public List<string> GalleryImages { get; set; } = new();
}
