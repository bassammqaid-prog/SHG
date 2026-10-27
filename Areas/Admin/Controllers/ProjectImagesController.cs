using Al_Shami_ِAISS_.Areas.Admin.ViewModels;
using Al_Shami_ِAISS_.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Al_Shami_ِAISS_.Areas.Admin.Controllers;

/// <summary>
/// Manages the public-facing project gallery inside the admin area.
/// The controller is intentionally thin: it accepts HTTP requests, validates them,
/// and delegates file handling and persistence to the project image service.
/// </summary>
[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public class ProjectImagesController : Controller
{
    private readonly ProjectImageService _projectImageService;
    private readonly ProjectService _projectService;
    private readonly IWebHostEnvironment _hostingEnvironment;

    /// <summary>
    /// Initializes the controller with the gallery service, project service, and hosting environment.
    /// </summary>
    public ProjectImagesController(ProjectImageService projectImageService, ProjectService projectService, IWebHostEnvironment hostingEnvironment)
    {
        _projectImageService = projectImageService;
        _projectService = projectService;
        _hostingEnvironment = hostingEnvironment;
    }

    /// <summary>
    /// Displays all gallery images for a selected project.
    /// </summary>
    public async Task<IActionResult> Index(int projectId)
    {
        var project = await _projectService.GetByIdAsync(projectId);
        if (project is null)
        {
            return NotFound();
        }

        var images = await _projectImageService.GetByProjectIdAsync(projectId);

        var viewModel = new ProjectImageListViewModel
        {
            ProjectId = project.Id,
            ProjectTitle = project.Title,
            Images = images
                .Select(image => new ProjectImageSummaryViewModel
                {
                    Id = image.Id,
                    ImageUrl = image.ImageUrl
                })
                .ToList()
        };

        return View(viewModel);
    }

    /// <summary>
    /// Displays the form used to add a gallery image to a project.
    /// </summary>
    public async Task<IActionResult> Create(int projectId)
    {
        var project = await _projectService.GetByIdAsync(projectId);
        if (project is null)
        {
            return NotFound();
        }

        var viewModel = new ProjectImageCreateViewModel
        {
            ProjectId = project.Id,
            ProjectTitle = project.Title
        };

        return View(viewModel);
    }

    /// <summary>
    /// Adds a new project gallery image.
    /// The form accepts either a local upload or a URL and the service decides where to store it.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int projectId, ProjectImageCreateViewModel model)
    {
        var project = await _projectService.GetByIdAsync(projectId);
        if (project is null)
        {
            return NotFound();
        }

        model.ProjectId = project.Id;
        model.ProjectTitle = project.Title;

        if (model.ImageFile is null && string.IsNullOrWhiteSpace(model.ImageUrl))
        {
            ModelState.AddModelError(string.Empty, "Please upload a file or provide an image URL.");
        }

        if (model.ImageFile is not null && model.ImageFile.Length == 0)
        {
            ModelState.AddModelError(nameof(model.ImageFile), "Please select a valid image file.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await _projectImageService.AddAsync(projectId, model.ImageFile, model.ImageUrl, _hostingEnvironment);
            TempData["SuccessMessage"] = "Project image uploaded successfully.";
            return RedirectToAction(nameof(Index), new { projectId });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    /// <summary>
    /// Displays a confirmation page for deleting a project gallery image.
    /// </summary>
    public async Task<IActionResult> Delete(int id)
    {
        var image = await _projectImageService.GetByIdAsync(id);
        if (image is null)
        {
            return NotFound();
        }

        var project = await _projectService.GetByIdAsync(image.ProjectId);

        var viewModel = new ProjectImageDeleteViewModel
        {
            Id = image.Id,
            ProjectId = image.ProjectId,
            ProjectTitle = project?.Title ?? "Unknown project",
            ImageUrl = image.ImageUrl
        };

        return View(viewModel);
    }

    /// <summary>
    /// Deletes a project image and removes the physical file when it was uploaded locally.
    /// </summary>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var image = await _projectImageService.GetByIdAsync(id);
        if (image is null)
        {
            return NotFound();
        }

        await _projectImageService.DeleteAsync(id, _hostingEnvironment);
        TempData["SuccessMessage"] = "Project image deleted successfully.";
        return RedirectToAction(nameof(Index), new { projectId = image.ProjectId });
    }
}
