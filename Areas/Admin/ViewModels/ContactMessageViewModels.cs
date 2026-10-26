using System.ComponentModel.DataAnnotations;

namespace Al_Shami_ِAISS_.Areas.Admin.ViewModels;

/// <summary>
/// Displays a contact message in a table list.
/// </summary>
public class ContactMessageListItemViewModel
{
    public int Id { get; set; }

    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Subject")]
    public string Subject { get; set; } = string.Empty;

    [Display(Name = "Created")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Displays full details of a contact message.
/// </summary>
public class ContactMessageDetailsViewModel
{
    public int Id { get; set; }

    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Phone")]
    public string? Phone { get; set; }

    [Display(Name = "Subject")]
    public string Subject { get; set; } = string.Empty;

    [Display(Name = "Message")]
    public string Message { get; set; } = string.Empty;

    [Display(Name = "Created")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Used when a user creates a contact message.
/// </summary>
public class ContactMessageCreateViewModel
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(200, ErrorMessage = "Full name cannot exceed 200 characters.")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [Display(Name = "Phone")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Subject is required.")]
    [StringLength(200, ErrorMessage = "Subject cannot exceed 200 characters.")]
    [Display(Name = "Subject")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Message is required.")]
    [StringLength(2000, ErrorMessage = "Message cannot exceed 2000 characters.")]
    [Display(Name = "Message")]
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Used when updating an existing contact message.
/// </summary>
public class ContactMessageEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(200, ErrorMessage = "Full name cannot exceed 200 characters.")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [Display(Name = "Phone")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Subject is required.")]
    [StringLength(200, ErrorMessage = "Subject cannot exceed 200 characters.")]
    [Display(Name = "Subject")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Message is required.")]
    [StringLength(2000, ErrorMessage = "Message cannot exceed 2000 characters.")]
    [Display(Name = "Message")]
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Used when confirming contact message deletion.
/// </summary>
public class ContactMessageDeleteViewModel
{
    public int Id { get; set; }

    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Subject")]
    public string Subject { get; set; } = string.Empty;

    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;
}
