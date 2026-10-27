using System.ComponentModel.DataAnnotations;

namespace Al_Shami_ِAISS_.ViewModels;

/// <summary>
/// Represents the data submitted by a user during the login process.
/// This ViewModel is intentionally focused on user sign-in input and not on the full identity database model.
/// </summary>
public class LoginViewModel
{
    /// <summary>
    /// Email address used to identify the account.
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Password used for authentication.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Controls whether the cookie should persist after the browser is closed.
    /// </summary>
    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
}
