using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Al_Shami_AISS_.Areas.Admin.ViewModels;

/// <summary>
/// Represents the admin product image listing page.
/// It contains the related product information and the gallery entries for that product.
/// </summary>
public class ProductImageListViewModel
{
    public int ProductId { get; set; }

    [Display(Name = "Product")]
    public string ProductName { get; set; } = string.Empty;

    public List<ProductImageSummaryViewModel> Images { get; set; } = new();
}

/// <summary>
/// Represents a single gallery image item rendered in the admin table.
/// </summary>
public class ProductImageSummaryViewModel
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}

/// <summary>
/// Used when an admin uploads a new product image.
/// This view model separates the form data from the database entity and keeps the HTTP layer clean.
/// </summary>
public class ProductImageCreateViewModel
{
    public int ProductId { get; set; }

    [Display(Name = "Product")]
    public string ProductName { get; set; } = string.Empty;

    [Display(Name = "Image File")]
    public IFormFile? ImageFile { get; set; }

    [Url(ErrorMessage = "Please provide a valid image URL.")]
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }
}

/// <summary>
/// Used when confirming the deletion of a product image.
/// </summary>
public class ProductImageDeleteViewModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}
