using Al_Shami_AISS_.Services;
using Al_Shami_AISS_.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

using Al_Shami_AISS_.Resources;
using Microsoft.Extensions.Localization;

namespace Al_Shami_AISS_.Controllers;

/// <summary>
/// Public-facing home controller for the company website.
/// It prepares the marketing pages and loads all dynamic content through the service layer.
/// </summary>
public class HomeController : Controller
{
    private readonly CategoryService _categoryService;
    private readonly ProductService _productService;
    private readonly ProjectService _projectService;
    private readonly IStringLocalizer<SharedResource> _localizer;


    /// <summary>
    /// Initializes the controller with the services required to render public pages.
    /// </summary>
    public HomeController(CategoryService categoryService, ProductService productService, ProjectService projectService, IStringLocalizer<SharedResource> localizer )
    {
        _categoryService = categoryService;
        _productService = productService;
        _projectService = projectService;
        _localizer = localizer;
    }

    /// <summary>
    /// Displays the main landing page with featured categories, products, and projects.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "الرئيسية";
        ViewData["MetaTitle"] = "S H G | حلول صناعية وتوريد";
        ViewData["MetaDescription"] = "S H G تقدم حلولاً صناعية وتقنية وتوريدية متكاملة للمشاريع التجارية والصناعية.";
        ViewData["MetaKeywords"] = "S H G, حلول صناعية, توريد, مشاريع, كهرباء";

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
                "حلول هندسية",
                "توريد المعدات الصناعية",
                "أنظمة السلامة",
                "البنية التحتية للطاقة",
                "الصيانة والدعم",
                "توفير قطع الغيار"
            },
            WhyChooseUs = new List<string>
            {
                "تنفيذ المشاريع المثبتة",
                "شركاء توريد صناعي معتمدين",
                "دعم العملاء السريع",
                "توريد وتسليم عالي الجودة"
            }
        };

        return View(viewModel);
    }

    /// <summary>
    /// Displays the company overview and mission content.
    /// </summary>
    public IActionResult About()
    {
        ViewData["Title"] = "من نحن";
        ViewData["MetaTitle"] = "من نحن | S H G";
        ViewData["MetaDescription"] = "تعرف على رؤية S H G ومهامها في تقديم حلول صناعية وتجارية موثوقة.";
        ViewData["MetaKeywords"] = "من نحن, S H G, رؤية, حلول";
        return View();
    }

    /// <summary>
    /// Keeps the default privacy page available if the app still references it.
    /// </summary>
    public IActionResult Privacy()
    {
        ViewData["Title"] = "سياسة الخصوصية";
        ViewData["MetaTitle"] = "سياسة الخصوصية | S H G";
        ViewData["MetaDescription"] = "اقرأ سياسة الخصوصية الخاصة بـ S H G وكيفية تعاملنا مع بيانات المستخدمين.";
        ViewData["MetaKeywords"] = "سياسة الخصوصية, S H G";
        return View();
    }




public IActionResult TestLocalization()
{
    var names = typeof(SharedResource)
        .Assembly
        .GetManifestResourceNames();

    return Content(string.Join("\n", names));
}
}
