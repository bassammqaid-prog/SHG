using Al_Shami_ِAISS_.Models;
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
    private readonly ProjectImageService _projectImageService;

    /// <summary>
    /// Initializes the controller using dependency injection.
    /// </summary>
    public ProjectsController(ProjectService projectService, ProjectImageService projectImageService)
    {
        _projectService = projectService;
        _projectImageService = projectImageService;
    }

    /// <summary>
    /// Displays the public portfolio listing.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "مشاريعنا";
        ViewData["MetaTitle"] = "مشاريعنا | AISS";
        ViewData["MetaDescription"] = "اكتشف مشاريعنا في الإنشاءات والحلول والأنظمة الصناعية.";
        ViewData["MetaKeywords"] = "مشاريع, حلول, بناء, AISS";

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
    /// The action supports both the legacy ID route and the new SEO-friendly slug route.
    /// </summary>
    [HttpGet("/projects/{slug}")]
    [HttpGet("/Projects/Details/{id:int}")]
    public async Task<IActionResult> Details(int? id, string? slug)
    {
        Project? project = null;

        if (!string.IsNullOrWhiteSpace(slug))
        {
            project = await _projectService.GetBySlugAsync(slug);
        }

        if (project == null && id.HasValue)
        {
            project = await _projectService.GetByIdAsync(id.Value);
        }

        if (project == null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(project.Slug))
        {
            project.Slug = SeoSlugService.GenerateSlug(project.Title);
        }

        if (string.IsNullOrWhiteSpace(project.CanonicalUrl))
        {
            project.CanonicalUrl = $"/projects/{project.Slug}";
        }

        ViewData["Title"] = project.MetaTitle ?? project.Title;
        ViewData["MetaTitle"] = project.MetaTitle ?? project.Title;
        ViewData["MetaDescription"] = project.MetaDescription ?? project.Description;
        ViewData["MetaKeywords"] = project.MetaKeywords ?? $"{project.Title},{project.ClientName},{project.Location}";
        ViewData["CanonicalUrl"] = $"{Request.Scheme}://{Request.Host}{project.CanonicalUrl}";
        ViewData["OgImage"] = project.ImageUrl ?? "/images/default-project.jpg";

        var galleryImages = await _projectImageService.GetByProjectIdAsync(project.Id);

        var viewModel = new ProjectDetailsViewModel
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            ImageUrl = project.ImageUrl,
            ClientName = project.ClientName,
            Location = project.Location,
            CompletionDate = project.CompletionDate,
            IsFeatured = project.IsFeatured,
            GalleryImages = galleryImages
                .Select(image => image.ImageUrl)
                .ToList()
        };

        return View(viewModel);
    }
}
