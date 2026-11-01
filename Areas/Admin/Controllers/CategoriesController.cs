using Al_Shami_ِAISS_.Areas.Admin.ViewModels;
using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Al_Shami_ِAISS_.Areas.Admin.Controllers;

/// <summary>
/// Handles category management within the Admin area.
/// The controller stays thin by delegating all business logic to the CategoryService.
/// This keeps the controller focused on HTTP actions while the service handles validation and database orchestration.
/// </summary>
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CategoriesController : Controller
{
    private readonly CategoryService _categoryService;

    /// <summary>
    /// Initializes the controller with the category service through dependency injection.
    /// </summary>
    public CategoriesController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Displays the list of categories.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllAsync();
        var viewModel = categories.Select(c => new CategoryListItemViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            IsActive = c.IsActive,
            CreatedAt = c.CreatedAt
        }).ToList();

        return View(viewModel);
    }

    /// <summary>
    /// Displays a category detail page.
    /// </summary>
    public async Task<IActionResult> Details(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        var viewModel = new CategoryDetailsViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            ProductCount = category.Products.Count
        };

        return View(viewModel);
    }

    /// <summary>
    /// Displays the create form for a category.
    /// </summary>
    public IActionResult Create()
    {
        return View(new CategoryCreateViewModel());
    }

    /// <summary>
    /// Processes category creation.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var category = new Category
        {
            Name = model.Name,
            Description = model.Description,
            IsActive = model.IsActive,
            MetaTitle = string.IsNullOrWhiteSpace(model.MetaTitle) ? model.Name : model.MetaTitle,
            MetaDescription = model.MetaDescription,
            MetaKeywords = model.MetaKeywords,
            Slug = string.IsNullOrWhiteSpace(model.Slug) ? SeoSlugService.GenerateSlug(model.Name) : model.Slug,
            CanonicalUrl = string.IsNullOrWhiteSpace(model.CanonicalUrl)
                ? $"/categories/{SeoSlugService.GenerateSlug(model.Slug ?? model.Name)}"
                : model.CanonicalUrl,
            CreatedAt = DateTime.UtcNow
        };

        await _categoryService.AddAsync(category);
        TempData["SuccessMessage"] = "Category created successfully.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays the edit form for a category.
    /// </summary>
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        var viewModel = new CategoryEditViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            MetaTitle = category.MetaTitle,
            MetaDescription = category.MetaDescription,
            MetaKeywords = category.MetaKeywords,
            Slug = category.Slug,
            CanonicalUrl = category.CanonicalUrl
        };

        return View(viewModel);
    }

    /// <summary>
    /// Processes category updates.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        category.Name = model.Name;
        category.Description = model.Description;
        category.IsActive = model.IsActive;
        category.MetaTitle = string.IsNullOrWhiteSpace(model.MetaTitle) ? model.Name : model.MetaTitle;
        category.MetaDescription = model.MetaDescription;
        category.MetaKeywords = model.MetaKeywords;
        category.Slug = string.IsNullOrWhiteSpace(model.Slug) ? SeoSlugService.GenerateSlug(model.Name) : model.Slug;
        category.CanonicalUrl = string.IsNullOrWhiteSpace(model.CanonicalUrl)
            ? $"/categories/{category.Slug}"
            : model.CanonicalUrl;

        await _categoryService.UpdateAsync(category);
        TempData["SuccessMessage"] = "Category updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays the delete confirmation form.
    /// </summary>
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        var viewModel = new CategoryDeleteViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive
        };

        return View(viewModel);
    }

    /// <summary>
    /// Processes category deletion.
    /// </summary>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _categoryService.DeleteAsync(id);
        TempData["SuccessMessage"] = "Category deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
