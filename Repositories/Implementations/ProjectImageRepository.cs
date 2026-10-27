using Al_Shami_ِAISS_.Data;
using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Al_Shami_ِAISS_.Repositories.Implementations;

/// <summary>
/// Entity Framework implementation of the project image repository.
/// This class is responsible for querying and persisting the ProjectImages table while the service layer focuses on validation and business logic.
/// </summary>
public class ProjectImageRepository : IProjectImageRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes the repository with the application database context.
    /// </summary>
    /// <param name="context">The EF Core context used for project gallery persistence.</param>
    public ProjectImageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProjectImage>> GetByProjectIdAsync(int projectId)
    {
        return await _context.ProjectImages
            .AsNoTracking()
            .Where(pi => pi.ProjectId == projectId)
            .OrderBy(pi => pi.Id)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<ProjectImage?> GetByIdAsync(int id)
    {
        return await _context.ProjectImages
            .AsNoTracking()
            .FirstOrDefaultAsync(pi => pi.Id == id);
    }

    /// <inheritdoc />
    public async Task AddAsync(ProjectImage projectImage)
    {
        await _context.ProjectImages.AddAsync(projectImage);
    }

    /// <inheritdoc />
    public void Update(ProjectImage projectImage)
    {
        _context.ProjectImages.Update(projectImage);
    }

    /// <inheritdoc />
    public void Delete(ProjectImage projectImage)
    {
        _context.ProjectImages.Remove(projectImage);
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
