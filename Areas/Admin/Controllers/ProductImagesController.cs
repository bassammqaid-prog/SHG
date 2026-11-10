using Al_Shami_AISS_.Areas.Admin.ViewModels;
using Al_Shami_AISS_.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Al_Shami_AISS_.Areas.Admin.Controllers;

/// <summary>
/// Handles product image management inside the Admin area.
/// This controller manages the product gallery and keeps file upload logic out of the UI layer.
/// </summary>
[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public class ProductImagesController : Controller
{
    private readonly ProductImageService _productImageService;
    private readonly ProductService _productService;
    private readonly IWebHostEnvironment _hostingEnvironment;

    /// <summary>
    /// Initializes the controller with the image service, product service, and web host environment.
    /// </summary>
    public ProductImagesController(ProductImageService productImageService, ProductService productService, IWebHostEnvironment hostingEnvironment)
    {
        _productImageService = productImageService;
        _productService = productService;
        _hostingEnvironment = hostingEnvironment;
    }

    /// <summary>
    /// Displays all uploaded images for a product.
    /// </summary>
    public async Task<IActionResult> Index(int productId)
    {
        var product = await _productService.GetByIdAsync(productId);
        if (product is null)
        {
            return NotFound();
        }

        var images = await _productImageService.GetByProductIdAsync(productId);

        var viewModel = new ProductImageListViewModel
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Images = images
                .Select(image => new ProductImageSummaryViewModel
                {
                    Id = image.Id,
                    ImageUrl = image.ImageUrl
                })
                .ToList()
        };

        return View(viewModel);
    }

    /// <summary>
    /// Displays the form used to add a new product image.
    /// </summary>
    public async Task<IActionResult> Create(int productId)
    {
        var product = await _productService.GetByIdAsync(productId);
        if (product is null)
        {
            return NotFound();
        }

        var viewModel = new ProductImageCreateViewModel
        {
            ProductId = product.Id,
            ProductName = product.Name
        };

        return View(viewModel);
    }

    /// <summary>
    /// Processes a new product image upload.
    /// This action accepts an uploaded file or a URL and stores it through the service layer.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int productId, ProductImageCreateViewModel model)
    {
        var product = await _productService.GetByIdAsync(productId);
        if (product is null)
        {
            return NotFound();
        }

        model.ProductId = product.Id;
        model.ProductName = product.Name;

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
            await _productImageService.AddAsync(productId, model.ImageFile, model.ImageUrl, _hostingEnvironment);
            TempData["SuccessMessage"] = "Product image uploaded successfully.";
            return RedirectToAction(nameof(Index), new { productId });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    /// <summary>
    /// Displays the image deletion confirmation screen.
    /// </summary>
    public async Task<IActionResult> Delete(int id)
    {
        var image = await _productImageService.GetByIdAsync(id);
        if (image is null)
        {
            return NotFound();
        }

        var product = await _productService.GetByIdAsync(image.ProductId);

        var viewModel = new ProductImageDeleteViewModel
        {
            Id = image.Id,
            ProductId = image.ProductId,
            ProductName = product?.Name ?? "Unknown product",
            ImageUrl = image.ImageUrl
        };

        return View(viewModel);
    }

    /// <summary>
    /// Deletes a product image and removes the associated file from the server when applicable.
    /// </summary>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var image = await _productImageService.GetByIdAsync(id);
        if (image is null)
        {
            return NotFound();
        }

        await _productImageService.DeleteAsync(id, _hostingEnvironment);
        TempData["SuccessMessage"] = "Product image deleted successfully.";
        return RedirectToAction(nameof(Index), new { productId = image.ProductId });
    }
}
