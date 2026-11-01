using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Al_Shami_ِAISS_.Areas.Admin.ViewModels;

/// <summary>
/// Displays a product in an admin table.
/// This view model is intentionally stripped down to only the properties needed by the list screen.
/// </summary>
public class ProductListItemViewModel
{
    public int Id { get; set; }

    [Display(Name = "Product Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Brand")]
    public string? Brand { get; set; }

    [Display(Name = "Price")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal Price { get; set; }

    [Display(Name = "Stock")]
    public int StockQuantity { get; set; }

    [Display(Name = "Featured")]
    public bool IsFeatured { get; set; }

    [Display(Name = "Category")]
    public string CategoryName { get; set; } = string.Empty;

    [Display(Name = "Created")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Displays all details of a product on the details page.
/// </summary>
public class ProductDetailsViewModel
{
    public int Id { get; set; }

    [Display(Name = "Product Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Price")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal Price { get; set; }

    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Product gallery images already associated with this product.
    /// They are displayed in the admin detail view so staff can manage the visual catalog.
    /// </summary>
    public List<string> GalleryImages { get; set; } = new();

    [Display(Name = "Model Number")]
    public string? ModelNumber { get; set; }

    [Display(Name = "Brand")]
    public string? Brand { get; set; }

    [Display(Name = "Stock Quantity")]
    public int StockQuantity { get; set; }

    [Display(Name = "Featured")]
    public bool IsFeatured { get; set; }

    [Display(Name = "Category")]
    public string CategoryName { get; set; } = string.Empty;

    [Display(Name = "Created")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Used when a user creates a product.
/// </summary>
public class ProductCreateViewModel
{
    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(200, ErrorMessage = "Product name cannot exceed 200 characters.")]
    [Display(Name = "Product Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Range(0.01, 9999999.99, ErrorMessage = "Price must be greater than zero.")]
    [Display(Name = "Price")]
    public decimal Price { get; set; }

    [Url(ErrorMessage = "Please enter a valid image URL.")]
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    [StringLength(100, ErrorMessage = "Model number cannot exceed 100 characters.")]
    [Display(Name = "Model Number")]
    public string? ModelNumber { get; set; }

    [StringLength(100, ErrorMessage = "Brand cannot exceed 100 characters.")]
    [Display(Name = "Brand")]
    public string? Brand { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
    [Display(Name = "Stock Quantity")]
    public int StockQuantity { get; set; }

    [Display(Name = "Is Featured?")]
    public bool IsFeatured { get; set; }

    [Required(ErrorMessage = "Please select a category.")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Display(Name = "Meta Title")]
    [StringLength(160, ErrorMessage = "Meta title cannot exceed 160 characters.")]
    public string? MetaTitle { get; set; }

    [Display(Name = "Meta Description")]
    [StringLength(300, ErrorMessage = "Meta description cannot exceed 300 characters.")]
    public string? MetaDescription { get; set; }

    [Display(Name = "Meta Keywords")]
    public string? MetaKeywords { get; set; }

    [Display(Name = "Slug")]
    [RegularExpression("^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must use lowercase letters, numbers, and hyphens only.")]
    public string? Slug { get; set; }

    [Display(Name = "Canonical URL")]
    [RegularExpression("^/?[a-zA-Z0-9/_-]+$", ErrorMessage = "Canonical URL must use a relative path such as /products/steel-door.")]
    [StringLength(500, ErrorMessage = "Canonical URL cannot exceed 500 characters.")]
    public string? CanonicalUrl { get; set; }

    public List<SelectListItem> CategoryOptions { get; set; } = new();
}

/// <summary>
/// Used when updating an existing product.
/// </summary>
public class ProductEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(200, ErrorMessage = "Product name cannot exceed 200 characters.")]
    [Display(Name = "Product Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Range(0.01, 9999999.99, ErrorMessage = "Price must be greater than zero.")]
    [Display(Name = "Price")]
    public decimal Price { get; set; }

    [Url(ErrorMessage = "Please enter a valid image URL.")]
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    [StringLength(100, ErrorMessage = "Model number cannot exceed 100 characters.")]
    [Display(Name = "Model Number")]
    public string? ModelNumber { get; set; }

    [StringLength(100, ErrorMessage = "Brand cannot exceed 100 characters.")]
    [Display(Name = "Brand")]
    public string? Brand { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
    [Display(Name = "Stock Quantity")]
    public int StockQuantity { get; set; }

    [Display(Name = "Is Featured?")]
    public bool IsFeatured { get; set; }

    [Required(ErrorMessage = "Please select a category.")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Display(Name = "Meta Title")]
    [StringLength(160, ErrorMessage = "Meta title cannot exceed 160 characters.")]
    public string? MetaTitle { get; set; }

    [Display(Name = "Meta Description")]
    [StringLength(300, ErrorMessage = "Meta description cannot exceed 300 characters.")]
    public string? MetaDescription { get; set; }

    [Display(Name = "Meta Keywords")]
    public string? MetaKeywords { get; set; }

    [Display(Name = "Slug")]
    [RegularExpression("^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug must use lowercase letters, numbers, and hyphens only.")]
    public string? Slug { get; set; }

    [Display(Name = "Canonical URL")]
    [RegularExpression("^/?[a-zA-Z0-9/_-]+$", ErrorMessage = "Canonical URL must use a relative path such as /products/steel-door.")]
    [StringLength(500, ErrorMessage = "Canonical URL cannot exceed 500 characters.")]
    public string? CanonicalUrl { get; set; }

    public List<SelectListItem> CategoryOptions { get; set; } = new();
}

/// <summary>
/// Used before deleting a product.
/// </summary>
public class ProductDeleteViewModel
{
    public int Id { get; set; }

    [Display(Name = "Product Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Brand")]
    public string? Brand { get; set; }

    [Display(Name = "Price")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal Price { get; set; }

    [Display(Name = "Stock Quantity")]
    public int StockQuantity { get; set; }

    [Display(Name = "Category")]
    public string CategoryName { get; set; } = string.Empty;
}
