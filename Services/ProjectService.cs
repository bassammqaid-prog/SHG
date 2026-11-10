using Al_Shami_AISS_.Models;
using Al_Shami_AISS_.Repositories.Interfaces;

namespace Al_Shami_AISS_.Services;

/// <summary>
/// Business service for project management.
/// This layer validates data and coordinates repository operations, keeping controllers thin.
/// </summary>
public class ProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IActivityLogService _activityLogService;

    /// <summary>
    /// Initializes the project service with a repository dependency.
    /// Dependency injection supplies the repository automatically.
    /// </summary>
    public ProjectService(IProjectRepository projectRepository, IActivityLogService activityLogService)
    {
        _projectRepository = projectRepository;
        _activityLogService = activityLogService;
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
    /// Retrieves a project using its SEO-friendly slug.
    /// </summary>
    public async Task<Project?> GetBySlugAsync(string slug)
    {
        return await _projectRepository.GetBySlugAsync(slug);
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

        project.Slug = await SeoSlugService.EnsureUniqueSlugAsync(project.Title, async slug => await _projectRepository.GetBySlugAsync(slug) != null);
        project.CanonicalUrl = $"/projects/{project.Slug}";
        project.MetaTitle ??= project.Title;
        project.MetaDescription ??= project.Description;
        project.MetaKeywords ??= $"{project.Title},{project.ClientName},{project.Location}";

        await _projectRepository.AddAsync(project);
        await _projectRepository.SaveChangesAsync();

        await _activityLogService.LogProjectCreatedAsync(project);

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

        project.Slug = await SeoSlugService.EnsureUniqueSlugAsync(project.Title, async slug => await _projectRepository.GetBySlugAsync(slug) != null, project.Slug);
        project.CanonicalUrl = $"/projects/{project.Slug}";
        project.MetaTitle ??= project.Title;
        project.MetaDescription ??= project.Description;
        project.MetaKeywords ??= $"{project.Title},{project.ClientName},{project.Location}";

        _projectRepository.Update(project);
        await _projectRepository.SaveChangesAsync();

        await _activityLogService.LogProjectUpdatedAsync(project);
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

        await _activityLogService.LogProjectDeletedAsync(project);
    }
}
