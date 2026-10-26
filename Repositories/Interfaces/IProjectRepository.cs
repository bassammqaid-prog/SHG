using Al_Shami_ِAISS_.Models;

namespace Al_Shami_ِAISS_.Repositories.Interfaces;

/// <summary>
/// Defines the repository contract for project data access.
/// This interface keeps the service layer independent from the EF Core implementation.
/// It is part of the repository pattern used throughout the application.
/// </summary>
public interface IProjectRepository
{
    /// <summary>
    /// Retrieves all projects in the database.
    /// </summary>
    Task<IEnumerable<Project>> GetAllAsync();

    /// <summary>
    /// Retrieves a single project by its identifier.
    /// </summary>
    Task<Project?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new project to the underlying data store.
    /// </summary>
    Task AddAsync(Project project);

    /// <summary>
    /// Marks an existing project as updated.
    /// </summary>
    void Update(Project project);

    /// <summary>
    /// Removes a project from the database.
    /// </summary>
    void Delete(Project project);

    /// <summary>
    /// Saves all pending database operations.
    /// </summary>
    Task<int> SaveChangesAsync();
}
