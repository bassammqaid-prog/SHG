using System.ComponentModel.DataAnnotations;

namespace Al_Shami_AISS_.ViewModels;

/// <summary>
/// Represents the public quote request form.
/// This model decouples the UI from the domain entity so the controller can validate and map values cleanly.
/// </summary>
public class QuoteRequestViewModel
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(200, ErrorMessage = "Full name cannot exceed 200 characters.")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Company name cannot exceed 200 characters.")]
    [Display(Name = "Company Name")]
    public string? CompanyName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [StringLength(50, ErrorMessage = "Phone number cannot exceed 50 characters.")]
    [Display(Name = "Phone")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Message is required.")]
    [StringLength(2000, ErrorMessage = "Message cannot exceed 2000 characters.")]
    [Display(Name = "Message")]
    public string Message { get; set; } = string.Empty;
}
