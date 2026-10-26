using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Repositories.Interfaces;

namespace Al_Shami_ِAISS_.Services;

/// <summary>
/// Business service for project management.
/// This layer validates data and coordinates repository operations, keeping controllers thin.
/// </summary>
public class ProjectService
{
    private readonly IProjectRepository _projectRepository;

    /// <summary>
    /// Initializes the project service with a repository dependency.
    /// Dependency injection supplies the repository automatically.
    /// </summary>
    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    /// <summary>
    /// Retrieves all projects for display in the admin dashboard or catalog.
    /// </summary>
    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _projectRepository.GetAllAsync();
    }

    /// <summary>
    /// Retrieves a single project by identifier.
    /// </summary>
    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _projectRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Creates a new project and saves it.
    /// </summary>
    public async Task<Project> AddAsync(Project project)
    {
        if (project == null)
        {
            throw new ArgumentNullException(nameof(project));
        }

        if (string.IsNullOrWhiteSpace(project.Title))
        {
            throw new InvalidOperationException("Project title is required.");
        }

        await _projectRepository.AddAsync(project);
        await _projectRepository.SaveChangesAsync();

        return project;
    }

    /// <summary>
    /// Updates an existing project.
    /// </summary>
    public async Task UpdateAsync(Project project)
    {
        if (project == null)
        {
            throw new ArgumentNullException(nameof(project));
        }

        if (string.IsNullOrWhiteSpace(project.Title))
        {
            throw new InvalidOperationException("Project title is required.");
        }

        _projectRepository.Update(project);
        await _projectRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a project by identifier.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);

        if (project == null)
        {
            throw new KeyNotFoundException($"Project with id {id} was not found.");
        }

        _projectRepository.Delete(project);
        await _projectRepository.SaveChangesAsync();
    }
}
