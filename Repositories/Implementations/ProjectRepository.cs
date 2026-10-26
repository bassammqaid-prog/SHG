using Al_Shami_ِAISS_.Data;
using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Al_Shami_ِAISS_.Repositories.Implementations;

/// <summary>
/// EF Core implementation of the project repository.
/// This class handles all project database operations and stays isolated from business logic.
/// </summary>
public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes the repository with the application DbContext.
    /// </summary>
    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _context.Projects
            .AsNoTracking()
            .OrderByDescending(p => p.CompletionDate ?? DateTime.MinValue)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <inheritdoc />
    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
    }

    /// <inheritdoc />
    public void Update(Project project)
    {
        _context.Projects.Update(project);
    }

    /// <inheritdoc />
    public void Delete(Project project)
    {
        _context.Projects.Remove(project);
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
