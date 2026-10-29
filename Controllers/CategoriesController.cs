using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Services;
using Microsoft.AspNetCore.Mvc;

namespace Al_Shami_ِAISS_.Controllers;

/// <summary>
/// Public controller for category pages.
/// It exposes SEO-friendly category routes and keeps the public catalog discoverable by search engines.
/// </summary>
public class CategoriesController : Controller
{
    private readonly CategoryService _categoryService;

    /// <summary>
    /// Initializes the controller with category business logic.
    /// </summary>
    public CategoriesController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Displays the public category listing page.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "الفئات";
        ViewData["MetaTitle"] = "الفئات | AISS";
        ViewData["MetaDescription"] = "استعرض الفئات الرئيسية للخدمات والمنتجات المتوفرة ضمن حلول AISS.";
        ViewData["MetaKeywords"] = "فئات, منتجات, AISS, حلول";

        var categories = await _categoryService.GetAllAsync();
        return View(categories.Where(c => c.IsActive).ToList());
    }

    /// <summary>
    /// Displays a category detail page using a user-friendly slug.
    /// The action supports both the legacy ID route and the SEO route pattern.
    /// </summary>
    [HttpGet("/categories/{slug}")]
    [HttpGet("/Categories/Details/{id:int}")]
    public async Task<IActionResult> Details(int? id, string? slug)
    {
        Category? category = null;

        if (!string.IsNullOrWhiteSpace(slug))
        {
            category = await _categoryService.GetBySlugAsync(slug);
        }

        if (category == null && id.HasValue)
        {
            category = await _categoryService.GetByIdAsync(id.Value);
        }

        if (category == null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(category.Slug))
        {
            category.Slug = SeoSlugService.GenerateSlug(category.Name);
        }

        if (string.IsNullOrWhiteSpace(category.CanonicalUrl))
        {
            category.CanonicalUrl = $"/categories/{category.Slug}";
        }

        ViewData["Title"] = category.MetaTitle ?? category.Name;
        ViewData["MetaTitle"] = category.MetaTitle ?? category.Name;
        ViewData["MetaDescription"] = category.MetaDescription ?? category.Description;
        ViewData["MetaKeywords"] = category.MetaKeywords ?? category.Name;
        ViewData["CanonicalUrl"] = $"{Request.Scheme}://{Request.Host}{category.CanonicalUrl}";

        return View(category);
    }
}
