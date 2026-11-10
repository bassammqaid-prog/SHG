using System.ComponentModel.DataAnnotations;

namespace Al_Shami_AISS_.Areas.Admin.ViewModels;

/// <summary>
/// Displays a quote request in the admin list table.
/// </summary>
public class QuoteRequestListItemViewModel
{
    public int Id { get; set; }

    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Company")]
    public string? CompanyName { get; set; }

    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Created")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Displays all details of a quote request.
/// </summary>
public class QuoteRequestDetailsViewModel
{
    public int Id { get; set; }

    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Company")]
    public string? CompanyName { get; set; }

    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Phone")]
    public string? Phone { get; set; }

    [Display(Name = "Message")]
    public string Message { get; set; } = string.Empty;

    [Display(Name = "Created")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Used when creating a new quote request.
/// </summary>
public class QuoteRequestCreateViewModel
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(200, ErrorMessage = "Full name cannot exceed 200 characters.")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Company name cannot exceed 200 characters.")]
    [Display(Name = "Company")]
    public string? CompanyName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [Display(Name = "Phone")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Message is required.")]
    [StringLength(2000, ErrorMessage = "Message cannot exceed 2000 characters.")]
    [Display(Name = "Message")]
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Used when updating an existing quote request.
/// </summary>
public class QuoteRequestEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(200, ErrorMessage = "Full name cannot exceed 200 characters.")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Company name cannot exceed 200 characters.")]
    [Display(Name = "Company")]
    public string? CompanyName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [Display(Name = "Phone")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Message is required.")]
    [StringLength(2000, ErrorMessage = "Message cannot exceed 2000 characters.")]
    [Display(Name = "Message")]
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Used before deleting a quote request.
/// </summary>
public class QuoteRequestDeleteViewModel
{
    public int Id { get; set; }

    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Company")]
    public string? CompanyName { get; set; }

    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;
}
