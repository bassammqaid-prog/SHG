using System.ComponentModel.DataAnnotations;

namespace Al_Shami_ِAISS_.Areas.Admin.ViewModels;

/// <summary>
/// A list item ViewModel used to display category data in an admin table.
/// ViewModels are used instead of entity classes so the UI can be tailored to display exactly what the Razor view needs.
/// </summary>
public class CategoryListItemViewModel
{
    public int Id { get; set; }

    [Display(Name = "Category Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; }

    [Display(Name = "Created")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Displays a single category in detail view.
/// </summary>
public class CategoryDetailsViewModel
{
    public int Id { get; set; }

    [Display(Name = "Category Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; }

    [Display(Name = "Created")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Products")]
    public int ProductCount { get; set; }
}

/// <summary>
/// Used when a user creates a new category.
/// This model provides validation and strongly-typed binding for the create form.
/// </summary>
public class CategoryCreateViewModel
{
    [Required(ErrorMessage = "Category name is required.")]
    [StringLength(200, ErrorMessage = "Category name cannot exceed 200 characters.")]
    [Display(Name = "Category Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Is Active")]
    public bool IsActive { get; set; } = true;

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
    [RegularExpression("^/?[a-zA-Z0-9/_-]+$", ErrorMessage = "Canonical URL must use a relative path such as /categories/steel-doors.")]
    [StringLength(500, ErrorMessage = "Canonical URL cannot exceed 500 characters.")]
    public string? CanonicalUrl { get; set; }
}

/// <summary>
/// Used when updating an existing category.
/// </summary>
public class CategoryEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Category name is required.")]
    [StringLength(200, ErrorMessage = "Category name cannot exceed 200 characters.")]
    [Display(Name = "Category Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Is Active")]
    public bool IsActive { get; set; }

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
    [RegularExpression("^/?[a-zA-Z0-9/_-]+$", ErrorMessage = "Canonical URL must use a relative path such as /categories/steel-doors.")]
    [StringLength(500, ErrorMessage = "Canonical URL cannot exceed 500 characters.")]
    public string? CanonicalUrl { get; set; }
}

/// <summary>
/// Used for deletion confirmation before removing a category.
/// </summary>
public class CategoryDeleteViewModel
{
    public int Id { get; set; }

    [Display(Name = "Category Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Is Active")]
    public bool IsActive { get; set; }
}
