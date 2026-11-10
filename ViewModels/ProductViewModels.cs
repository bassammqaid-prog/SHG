using System.ComponentModel.DataAnnotations;

namespace Al_Shami_AISS_.ViewModels;

/// <summary>
/// Represents a category option used for product filtering.
/// </summary>
public class CategoryFilterViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Represents a product card shown in the product listing page.
/// </summary>
public class ProductCardViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Brand { get; set; }

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public string CategoryName { get; set; } = string.Empty;
}

/// <summary>
/// View model used by the Products/Index page.
/// </summary>
public class ProductListViewModel
{
    public List<ProductCardViewModel> Products { get; set; } = [];

    public List<CategoryFilterViewModel> Categories { get; set; } = [];

    public string? SearchTerm { get; set; }

    public int? CategoryId { get; set; }

    public int PageNumber { get; set; }

    public int TotalPages { get; set; }
}

/// <summary>
/// View model used by the product details page.
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

    public List<string> GalleryImages { get; set; } = [];
}