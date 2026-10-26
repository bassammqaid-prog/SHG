using System.ComponentModel.DataAnnotations;

namespace Al_Shami_ِAISS_.ViewModels;

/// <summary>
/// Represents the public product listing page.
/// A dedicated view model lets the controller tailor the product list for marketing and search scenarios,
/// while keeping the database entity hidden from the UI layer.
/// </summary>
public class ProductListViewModel
{
    /// <summary>
    /// All products matching the current filters.
    /// </summary>
    public List<ProductCardViewModel> Products { get; set; } = new();

    /// <summary>
    /// Categories used for filtering the product list.
    /// </summary>
    public List<CategoryFilterViewModel> Categories { get; set; } = new();

    /// <summary>
    /// Current search term entered by the visitor.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Current category filter selection.
    /// </summary>
    public int? CategoryId { get; set; }

    /// <summary>
    /// Current page index for pagination.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Total number of pages available.
    /// </summary>
    public int TotalPages { get; set; }
}

/// <summary>
/// Represents one product tile displayed on the listing page.
/// </summary>
public class ProductCardViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}

/// <summary>
/// Represents a category option for the filter dropdown.
/// </summary>
public class CategoryFilterViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Represents the public product detail page.
/// </summary>
public class ProductDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? ModelNumber { get; set; }
    public string? Brand { get; set; }
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public List<string> GalleryImages { get; set; } = new();
}
