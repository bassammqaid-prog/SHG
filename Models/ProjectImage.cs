namespace Al_Shami_ِAISS_.Models;

/// <summary>
/// Represents an additional image associated with a project gallery.
/// Many industrial and commercial projects need more than one visual reference,
/// so a project can contain several gallery images while still keeping a main cover image.
/// </summary>
public class ProjectImage
{
    /// <summary>
    /// Primary key for the project image record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The stored URL or relative path to the image file.
    /// This value is saved in the database so the UI can render the image without exposing the physical file path.
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key that links the image to its owning project.
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Navigation property used by EF Core to traverse back to the project.
    /// This allows the ORM to materialize the related project when the gallery is loaded.
    /// </summary>
    public Project Project { get; set; } = null!;
}
