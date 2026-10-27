using Al_Shami_ِAISS_.Areas.Admin.ViewModels;
using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Al_Shami_ِAISS_.Areas.Admin.Controllers;

/// <summary>
/// Displays the administrative dashboard for the business.
/// This controller is intentionally thin and delegates data retrieval to services,
/// which keeps the controller focused on HTTP concerns and model preparation.
/// </summary>
[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public class DashboardController : Controller
{
    private readonly CategoryService _categoryService;
    private readonly ProductService _productService;
    private readonly ProjectService _projectService;
    private readonly ContactMessageService _contactMessageService;
    private readonly QuoteRequestService _quoteRequestService;
    private readonly IActivityLogService _activityLogService;
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Initializes the dashboard controller with the required service dependencies.
    /// Dependency injection provides these services through the ASP.NET Core container.
    /// </summary>
    public DashboardController(
        CategoryService categoryService,
        ProductService productService,
        ProjectService projectService,
        ContactMessageService contactMessageService,
        QuoteRequestService quoteRequestService,
        IActivityLogService activityLogService,
        UserManager<ApplicationUser> userManager)
    {
        _categoryService = categoryService;
        _productService = productService;
        _projectService = projectService;
        _contactMessageService = contactMessageService;
        _quoteRequestService = quoteRequestService;
        _activityLogService = activityLogService;
        _userManager = userManager;
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
        var activityLogs = await _activityLogService.GetRecentAsync(5);

        var viewModel = new DashboardViewModel
        {
            TotalCategories = categories.Count(),
            TotalProducts = products.Count(),
            TotalProjects = projects.Count(),
            TotalContactMessages = contactMessages.Count(),
            TotalQuoteRequests = quoteRequests.Count(),
            TotalUsers = await _userManager.Users.CountAsync(),
            TotalActivityLogs = (await _activityLogService.GetAllAsync()).Count(),
            RecentActivityLogs = activityLogs
                .Select(log => new RecentNotificationViewModel
                {
                    Id = log.Id,
                    Title = log.Action,
                    Detail = log.Description,
                    CreatedAt = log.CreatedAt
                })
                .ToList(),
            RecentContactMessages = contactMessages
                .OrderByDescending(message => message.CreatedAt)
                .Take(5)
                .Select(message => new RecentNotificationViewModel
                {
                    Id = message.Id,
                    Title = message.FullName,
                    Detail = message.Subject,
                    CreatedAt = message.CreatedAt
                })
                .ToList(),
            RecentQuoteRequests = quoteRequests
                .OrderByDescending(request => request.CreatedAt)
                .Take(5)
                .Select(request => new RecentNotificationViewModel
                {
                    Id = request.Id,
                    Title = request.FullName,
                    Detail = request.CompanyName ?? "No company provided",
                    CreatedAt = request.CreatedAt
                })
                .ToList()
        };

        return View(viewModel);
    }
}
