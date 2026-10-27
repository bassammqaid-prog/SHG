using Microsoft.AspNetCore.Identity;

namespace Al_Shami_ِAISS_.Models;

/// <summary>
/// Represents the authenticated application user.
/// This class extends ASP.NET Core Identity's built-in user entity so the application can store
/// custom business data such as the user's personal name and active/inactive flag.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// The user's first name as displayed in the application and admin screens.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// The user's last name.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Indicates whether the user account is active.
    /// Inactive users can be blocked from signing in without deleting their records.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// The moment when the account was first created.
    /// This is useful for auditing and for future reporting features.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
