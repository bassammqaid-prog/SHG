using Al_Shami_ِAISS_.Services;
using Al_Shami_ِAISS_.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Al_Shami_ِAISS_.Controllers;

/// <summary>
/// Public product catalog controller.
/// This controller is responsible only for HTTP requests and ViewModel preparation.
/// It delegates all data access and business logic to the ProductService and CategoryService.
/// </summary>
public class ProductsController : Controller
{
    private const int PageSize = 6;

    private readonly ProductService _productService;
    private readonly CategoryService _categoryService;

    /// <summary>
    /// Initializes the product controller with its service dependencies.
    /// ASP.NET Core injects these services automatically through dependency injection.
    /// </summary>
    public ProductsController(ProductService productService, CategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    /// <summary>
    /// Displays the public product catalog with optional search and category filtering.
    /// </summary>
    public async Task<IActionResult> Index(string? searchTerm, int? categoryId, int pageNumber = 1)
    {
        var products = await _productService.GetAllAsync();
        var categories = await _categoryService.GetAllAsync();

        var filteredProducts = products
            .Where(p => p.Category != null)
            .Where(p => string.IsNullOrWhiteSpace(searchTerm) || p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .Where(p => !categoryId.HasValue || p.CategoryId == categoryId.Value)
            .OrderBy(p => p.Name)
            .ToList();

        var totalPages = Math.Max(1, (int)Math.Ceiling((double)filteredProducts.Count / PageSize));
        var currentPage = Math.Clamp(pageNumber, 1, totalPages);

        var pagedProducts = filteredProducts
            .Skip((currentPage - 1) * PageSize)
            .Take(PageSize)
            .Select(p => new ProductCardViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Brand = p.Brand,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryName = p.Category?.Name ?? "Uncategorized"
            })
            .ToList();

        var viewModel = new ProductListViewModel
        {
            Products = pagedProducts,
            Categories = categories
                .Where(c => c.IsActive)
                .Select(c => new CategoryFilterViewModel { Id = c.Id, Name = c.Name })
                .ToList(),
            SearchTerm = searchTerm,
            CategoryId = categoryId,
            PageNumber = currentPage,
            TotalPages = totalPages
        };

        return View(viewModel);
    }

    /// <summary>
    /// Displays the product details page for a selected item.
    /// </summary>
    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        var galleryImages = product.ProductImages
            .Select(pi => pi.ImageUrl)
            .Where(url => !string.IsNullOrWhiteSpace(url))
            .ToList();

        if (!galleryImages.Any() && !string.IsNullOrWhiteSpace(product.ImageUrl))
        {
            galleryImages.Add(product.ImageUrl);
        }

        var viewModel = new ProductDetailsViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            ModelNumber = product.ModelNumber,
            Brand = product.Brand,
            Price = product.Price,
            CategoryName = product.Category?.Name ?? "Uncategorized",
            StockQuantity = product.StockQuantity,
            GalleryImages = galleryImages
        };

        return View(viewModel);
    }
}
