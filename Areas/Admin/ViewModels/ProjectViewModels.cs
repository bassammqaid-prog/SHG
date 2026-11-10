using System.ComponentModel.DataAnnotations;

namespace Al_Shami_AISS_.Areas.Admin.ViewModels;

/// <summary>
/// A lightweight list model used to display projects in the admin grid.
/// </summary>
public class ProjectListItemViewModel
{
    public int Id { get; set; }

    [Display(Name = "Project Title")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Client")]
    public string? ClientName { get; set; }

    [Display(Name = "Location")]
    public string? Location { get; set; }

    [Display(Name = "Completion Date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? CompletionDate { get; set; }

    [Display(Name = "Featured")]
    public bool IsFeatured { get; set; }
}

/// <summary>
/// View model used on the project details page.
/// </summary>
public class ProjectDetailsViewModel
{
    public int Id { get; set; }

    [Display(Name = "Project Title")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Client")]
    public string? ClientName { get; set; }

    [Display(Name = "Location")]
    public string? Location { get; set; }

    [Display(Name = "Completion Date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? CompletionDate { get; set; }

    [Display(Name = "Featured")]
    public bool IsFeatured { get; set; }
}

/// <summary>
/// Used during project creation.
/// </summary>
public class ProjectCreateViewModel
{
    [Required(ErrorMessage = "Project title is required.")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    [Display(Name = "Project Title")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Url(ErrorMessage = "Please enter a valid image URL.")]
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    [StringLength(200, ErrorMessage = "Client name cannot exceed 200 characters.")]
    [Display(Name = "Client")]
    public string? ClientName { get; set; }

    [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters.")]
    [Display(Name = "Location")]
    public string? Location { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Completion Date")]
    public DateTime? CompletionDate { get; set; }

    [Display(Name = "Featured")]
    public bool IsFeatured { get; set; }

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
    [RegularExpression("^/?[a-zA-Z0-9/_-]+$", ErrorMessage = "Canonical URL must use a relative path such as /projects/industrial-power-station.")]
    [StringLength(500, ErrorMessage = "Canonical URL cannot exceed 500 characters.")]
    public string? CanonicalUrl { get; set; }
}

/// <summary>
/// Used when updating an existing project.
/// </summary>
public class ProjectEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Project title is required.")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    [Display(Name = "Project Title")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Url(ErrorMessage = "Please enter a valid image URL.")]
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    [StringLength(200, ErrorMessage = "Client name cannot exceed 200 characters.")]
    [Display(Name = "Client")]
    public string? ClientName { get; set; }

    [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters.")]
    [Display(Name = "Location")]
    public string? Location { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Completion Date")]
    public DateTime? CompletionDate { get; set; }

    [Display(Name = "Featured")]
    public bool IsFeatured { get; set; }

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
    [RegularExpression("^/?[a-zA-Z0-9/_-]+$", ErrorMessage = "Canonical URL must use a relative path such as /projects/industrial-power-station.")]
    [StringLength(500, ErrorMessage = "Canonical URL cannot exceed 500 characters.")]
    public string? CanonicalUrl { get; set; }
}

/// <summary>
/// Used before deleting a project.
/// </summary>
public class ProjectDeleteViewModel
{
    public int Id { get; set; }

    [Display(Name = "Project Title")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Client")]
    public string? ClientName { get; set; }

    [Display(Name = "Location")]
    public string? Location { get; set; }

    [Display(Name = "Completion Date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? CompletionDate { get; set; }
}
