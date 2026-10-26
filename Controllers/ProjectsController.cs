using Al_Shami_ِAISS_.Services;
using Al_Shami_ِAISS_.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Al_Shami_ِAISS_.Controllers;

/// <summary>
/// Public controller for the project portfolio.
/// It keeps the UI layer free from entity objects and passes only ViewModels to Razor.
/// </summary>
public class ProjectsController : Controller
{
    private readonly ProjectService _projectService;

    /// <summary>
    /// Initializes the controller using dependency injection.
    /// </summary>
    public ProjectsController(ProjectService projectService)
    {
        _projectService = projectService;
    }

    /// <summary>
    /// Displays the public portfolio listing.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var projects = await _projectService.GetAllAsync();

        var viewModel = new ProjectListViewModel
        {
            Projects = projects
                .OrderByDescending(p => p.CompletionDate ?? DateTime.MinValue)
                .Select(p => new ProjectCardViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    ClientName = p.ClientName,
                    Location = p.Location,
                    ImageUrl = p.ImageUrl
                })
                .ToList()
        };

        return View(viewModel);
    }

    /// <summary>
    /// Displays project details for a single selected project.
    /// </summary>
    public async Task<IActionResult> Details(int id)
    {
        var project = await _projectService.GetByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        var viewModel = new ProjectDetailsViewModel
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            ImageUrl = project.ImageUrl,
            ClientName = project.ClientName,
            Location = project.Location,
            CompletionDate = project.CompletionDate,
            IsFeatured = project.IsFeatured
        };

        return View(viewModel);
    }
}
