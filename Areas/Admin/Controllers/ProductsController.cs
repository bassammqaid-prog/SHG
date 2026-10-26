using Al_Shami_ِAISS_.Areas.Admin.ViewModels;
using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Al_Shami_ِAISS_.Areas.Admin.Controllers;

/// <summary>
/// Handles product administration inside the Admin area.
/// Product-related create, edit, detail, and delete actions all flow through this controller.
/// </summary>
[Area("Admin")]
public class ProductsController : Controller
{
    private readonly ProductService _productService;
    private readonly CategoryService _categoryService;

    /// <summary>
    /// Initializes the controller with product and category services.
    /// </summary>
    public ProductsController(ProductService productService, CategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    /// <summary>
    /// Displays the list of products.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllAsync();
        var viewModel = products.Select(p => new ProductListItemViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Brand = p.Brand,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            IsFeatured = p.IsFeatured,
            CategoryName = p.Category?.Name ?? "Unassigned",
            CreatedAt = p.CreatedAt
        }).ToList();

        return View(viewModel);
    }

    /// <summary>
    /// Displays product details.
    /// </summary>
    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        var viewModel = new ProductDetailsViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            ModelNumber = product.ModelNumber,
            Brand = product.Brand,
            StockQuantity = product.StockQuantity,
            IsFeatured = product.IsFeatured,
            CategoryName = product.Category?.Name ?? "Unassigned",
            CreatedAt = product.CreatedAt
        };

        return View(viewModel);
    }

    /// <summary>
    /// Displays the create form with category options.
    /// </summary>
    public async Task<IActionResult> Create()
    {
        var model = new ProductCreateViewModel
        {
            CategoryOptions = await GetCategorySelectListAsync()
        };

        return View(model);
    }

    /// <summary>
    /// Processes product creation and validates the form.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateViewModel model)
    {
        model.CategoryOptions = await GetCategorySelectListAsync();

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var product = new Product
        {
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            ImageUrl = model.ImageUrl,
            ModelNumber = model.ModelNumber,
            Brand = model.Brand,
            StockQuantity = model.StockQuantity,
            IsFeatured = model.IsFeatured,
            CategoryId = model.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        await _productService.AddAsync(product);
        TempData["SuccessMessage"] = "Product created successfully.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays the edit form.
    /// </summary>
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        var viewModel = new ProductEditViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            ModelNumber = product.ModelNumber,
            Brand = product.Brand,
            StockQuantity = product.StockQuantity,
            IsFeatured = product.IsFeatured,
            CategoryId = product.CategoryId,
            CategoryOptions = await GetCategorySelectListAsync()
        };

        return View(viewModel);
    }

    /// <summary>
    /// Processes product update.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductEditViewModel model)
    {
        model.CategoryOptions = await GetCategorySelectListAsync();

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        product.Name = model.Name;
        product.Description = model.Description;
        product.Price = model.Price;
        product.ImageUrl = model.ImageUrl;
        product.ModelNumber = model.ModelNumber;
        product.Brand = model.Brand;
        product.StockQuantity = model.StockQuantity;
        product.IsFeatured = model.IsFeatured;
        product.CategoryId = model.CategoryId;

        await _productService.UpdateAsync(product);
        TempData["SuccessMessage"] = "Product updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays delete confirmation.
    /// </summary>
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        var viewModel = new ProductDeleteViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Brand = product.Brand,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryName = product.Category?.Name ?? "Unassigned"
        };

        return View(viewModel);
    }

    /// <summary>
    /// Processes product deletion.
    /// </summary>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _productService.DeleteAsync(id);
        TempData["SuccessMessage"] = "Product deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Creates a category dropdown list for forms.
    /// </summary>
    private async Task<List<SelectListItem>> GetCategorySelectListAsync()
    {
        var categories = await _categoryService.GetAllAsync();
        return categories
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .ToList();
    }
}
