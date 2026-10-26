using Al_Shami_ِAISS_.Services;
using Al_Shami_ِAISS_.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Al_Shami_ِAISS_.Controllers;

/// <summary>
/// Public-facing home controller for the company website.
/// It prepares the marketing pages and loads all dynamic content through the service layer.
/// </summary>
public class HomeController : Controller
{
    private readonly CategoryService _categoryService;
    private readonly ProductService _productService;
    private readonly ProjectService _projectService;

    /// <summary>
    /// Initializes the controller with the services required to render public pages.
    /// </summary>
    public HomeController(CategoryService categoryService, ProductService productService, ProjectService projectService)
    {
        _categoryService = categoryService;
        _productService = productService;
        _projectService = projectService;
    }

    /// <summary>
    /// Displays the main landing page with featured categories, products, and projects.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var categories = (await _categoryService.GetAllAsync())
            .Where(c => c.IsActive)
            .Take(4)
            .ToList();

        var featuredProducts = (await _productService.GetAllAsync())
            .Where(p => p.IsFeatured)
            .Take(4)
            .ToList();

        var featuredProjects = (await _projectService.GetAllAsync())
            .Where(p => p.IsFeatured)
            .Take(3)
            .ToList();

        var viewModel = new HomeViewModel
        {
            FeaturedCategories = categories
                .Select(c => new HomeCategoryItemViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToList(),
            FeaturedProducts = featuredProducts
                .Select(p => new HomeProductItemViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Brand = p.Brand,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    CategoryName = p.Category?.Name ?? "General"
                })
                .ToList(),
            FeaturedProjects = featuredProjects
                .Select(p => new HomeProjectItemViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    ClientName = p.ClientName,
                    Location = p.Location,
                    ImageUrl = p.ImageUrl
                })
                .ToList(),
            Services = new List<string>
            {
                "Engineering Solutions",
                "Industrial Equipment Supply",
                "Safety Systems",
                "Energy Infrastructure",
                "Maintenance and Support"
            },
            WhyChooseUs = new List<string>
            {
                "Proven project execution",
                "Certified industrial supply partners",
                "Responsive customer support",
                "Quality-first sourcing and delivery"
            }
        };

        return View(viewModel);
    }

    /// <summary>
    /// Displays the company overview and mission content.
    /// </summary>
    public IActionResult About()
    {
        return View();
    }

    /// <summary>
    /// Keeps the default privacy page available if the app still references it.
    /// </summary>
    public IActionResult Privacy()
    {
        return View();
    }
}
