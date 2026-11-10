using Al_Shami_AISS_.Models;

namespace Al_Shami_AISS_.Repositories.Interfaces;

/// <summary>
/// Defines the persistence contract for project gallery images.
/// The repository pattern keeps EF Core details out of the application service layer.
/// This separation ensures controllers and services depend on an abstraction rather than a concrete database implementation.
/// </summary>
public interface IProjectImageRepository
{
    /// <summary>
    /// Retrieves all gallery images attached to a specific project.
    /// </summary>
    /// <param name="projectId">The project identifier.</param>
    /// <returns>A collection of images for the selected project.</returns>
    Task<IEnumerable<ProjectImage>> GetByProjectIdAsync(int projectId);

    /// <summary>
    /// Retrieves a single gallery image by its identifier.
    /// </summary>
    /// <param name="id">The project image identifier.</param>
    /// <returns>The matching record or null when no row exists.</returns>
    Task<ProjectImage?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new project image record to the underlying data store.
    /// </summary>
    /// <param name="projectImage">The image record to persist.</param>
    Task AddAsync(ProjectImage projectImage);

    /// <summary>
    /// Marks an existing project image as updated.
    /// </summary>
    /// <param name="projectImage">The updated image entity.</param>
    void Update(ProjectImage projectImage);

    /// <summary>
    /// Marks a project image record for deletion.
    /// </summary>
    /// <param name="projectImage">The image record to remove.</param>
    void Delete(ProjectImage projectImage);

    /// <summary>
    /// Saves all pending database operations for the project gallery.
    /// </summary>
    /// <returns>The number of rows affected.</returns>
    Task<int> SaveChangesAsync();
}
