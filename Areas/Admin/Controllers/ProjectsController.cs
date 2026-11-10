// -----------------------------------------------------------------------------
//  File: ProjectsController.cs
//  Purpose: Admin CRUD controller for project management.
// -----------------------------------------------------------------------------

using Al_Shami_AISS_.Areas.Admin.ViewModels;
using Al_Shami_AISS_.Models;
using Al_Shami_AISS_.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Al_Shami_AISS_.Areas.Admin.Controllers;

/// <summary>
/// Manages project CRUD operations in the admin area.
/// The controller passes data to the service layer, which keeps business work separate from the UI.
/// </summary>
[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public class ProjectsController : Controller
{
    private readonly ProjectService _projectService;
    private readonly ProjectImageService _projectImageService;
    private readonly IWebHostEnvironment _hostingEnvironment;

    /// <summary>
    /// Initializes the controller with the project service.
    /// </summary>
    /// <param name="projectService">Service used for project CRUD operations.</param>
    /// <param name="projectImageService">Service used to save and manage project images.</param>
    /// <param name="hostingEnvironment">Hosting environment used to resolve web root paths for uploads.</param>
    public ProjectsController(ProjectService projectService, ProjectImageService projectImageService, IWebHostEnvironment hostingEnvironment)
    {
        _projectService = projectService;
        _projectImageService = projectImageService;
        _hostingEnvironment = hostingEnvironment;
    }

    /// <summary>
    /// Lists all projects.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var projects = await _projectService.GetAllAsync();
        var viewModel = projects.Select(p => new ProjectListItemViewModel
        {
            Id = p.Id,
            Title = p.Title,
            ClientName = p.ClientName,
            Location = p.Location,
            CompletionDate = p.CompletionDate,
            IsFeatured = p.IsFeatured
        }).ToList();

        return View(viewModel);
    }

    /// <summary>
    /// Displays project details.
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

    /// <summary>
    /// Displays the create form.
    /// </summary>
    public IActionResult Create()
    {
        return View(new ProjectCreateViewModel());
    }

    /// <summary>
    /// Processes project creation.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProjectCreateViewModel model)
    {
        if (model.UploadedImage is not null && model.UploadedImage.Length == 0)
        {
            ModelState.AddModelError(nameof(model.UploadedImage), "Please select a valid image file.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var project = new Project
        {
            Title = model.Title,
            Description = model.Description,
            ImageUrl = model.ImageUrl,
            ClientName = model.ClientName,
            Location = model.Location,
            CompletionDate = model.CompletionDate,
            IsFeatured = model.IsFeatured,
            MetaTitle = string.IsNullOrWhiteSpace(model.MetaTitle) ? model.Title : model.MetaTitle,
            MetaDescription = model.MetaDescription,
            MetaKeywords = model.MetaKeywords,
            Slug = string.IsNullOrWhiteSpace(model.Slug) ? SeoSlugService.GenerateSlug(model.Title) : model.Slug,
            CanonicalUrl = string.IsNullOrWhiteSpace(model.CanonicalUrl)
                ? $"/projects/{SeoSlugService.GenerateSlug(model.Slug ?? model.Title)}"
                : model.CanonicalUrl
        };

        try
        {
            await _projectService.AddAsync(project);

            if (model.UploadedImage is not null)
            {
                var projectImage = await _projectImageService.AddAsync(project.Id, model.UploadedImage, null, _hostingEnvironment);
                project.ImageUrl = projectImage.ImageUrl;
                await _projectService.UpdateAsync(project);
            }
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(nameof(model.UploadedImage), ex.Message);
            return View(model);
        }

        TempData["SuccessMessage"] = "Project created successfully.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays the edit form.
    /// </summary>
    public async Task<IActionResult> Edit(int id)
    {
        var project = await _projectService.GetByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        var viewModel = new ProjectEditViewModel
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            ImageUrl = project.ImageUrl,
            ClientName = project.ClientName,
            Location = project.Location,
            CompletionDate = project.CompletionDate,
            IsFeatured = project.IsFeatured,
            MetaTitle = project.MetaTitle,
            MetaDescription = project.MetaDescription,
            MetaKeywords = project.MetaKeywords,
            Slug = project.Slug,
            CanonicalUrl = project.CanonicalUrl
        };

        return View(viewModel);
    }

    /// <summary>
    /// Processes project updates.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProjectEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var project = await _projectService.GetByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        project.Title = model.Title;
        project.Description = model.Description;
        project.ImageUrl = model.ImageUrl;
        project.ClientName = model.ClientName;
        project.Location = model.Location;
        project.CompletionDate = model.CompletionDate;
        project.IsFeatured = model.IsFeatured;
        project.MetaTitle = string.IsNullOrWhiteSpace(model.MetaTitle) ? model.Title : model.MetaTitle;
        project.MetaDescription = model.MetaDescription;
        project.MetaKeywords = model.MetaKeywords;
        project.Slug = string.IsNullOrWhiteSpace(model.Slug) ? SeoSlugService.GenerateSlug(model.Title) : model.Slug;
        project.CanonicalUrl = string.IsNullOrWhiteSpace(model.CanonicalUrl)
            ? $"/projects/{project.Slug}"
            : model.CanonicalUrl;

        await _projectService.UpdateAsync(project);
        TempData["SuccessMessage"] = "Project updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays delete confirmation.
    /// </summary>
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _projectService.GetByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        var viewModel = new ProjectDeleteViewModel
        {
            Id = project.Id,
            Title = project.Title,
            ClientName = project.ClientName,
            Location = project.Location,
            CompletionDate = project.CompletionDate
        };

        return View(viewModel);
    }

    /// <summary>
    /// Processes project deletion.
    /// </summary>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _projectService.DeleteAsync(id);
        TempData["SuccessMessage"] = "Project deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
