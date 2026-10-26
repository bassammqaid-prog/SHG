using Al_Shami_ِAISS_.Areas.Admin.ViewModels;
using Al_Shami_ِAISS_.Services;
using Microsoft.AspNetCore.Mvc;

namespace Al_Shami_ِAISS_.Areas.Admin.Controllers;

/// <summary>
/// Displays the administrative dashboard for the business.
/// This controller is intentionally thin and delegates data retrieval to services,
/// which keeps the controller focused on HTTP concerns and model preparation.
/// </summary>
[Area("Admin")]
public class DashboardController : Controller
{
    private readonly CategoryService _categoryService;
    private readonly ProductService _productService;
    private readonly ProjectService _projectService;
    private readonly ContactMessageService _contactMessageService;
    private readonly QuoteRequestService _quoteRequestService;

    /// <summary>
    /// Initializes the dashboard controller with the required service dependencies.
    /// Dependency injection provides these services through the ASP.NET Core container.
    /// </summary>
    public DashboardController(
        CategoryService categoryService,
        ProductService productService,
        ProjectService projectService,
        ContactMessageService contactMessageService,
        QuoteRequestService quoteRequestService)
    {
        _categoryService = categoryService;
        _productService = productService;
        _projectService = projectService;
        _contactMessageService = contactMessageService;
        _quoteRequestService = quoteRequestService;
    }

    /// <summary>
    /// Renders the admin dashboard and calculates the summary counters.
    /// The controller does not touch the database directly; it delegates to services.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllAsync();
        var products = await _productService.GetAllAsync();
        var projects = await _projectService.GetAllAsync();
        var contactMessages = await _contactMessageService.GetAllAsync();
        var quoteRequests = await _quoteRequestService.GetAllAsync();

        var viewModel = new DashboardViewModel
        {
            TotalCategories = categories.Count(),
            TotalProducts = products.Count(),
            TotalProjects = projects.Count(),
            TotalContactMessages = contactMessages.Count(),
            TotalQuoteRequests = quoteRequests.Count()
        };

        return View(viewModel);
    }
}
