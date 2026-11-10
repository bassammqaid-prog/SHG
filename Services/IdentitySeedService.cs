using Al_Shami_AISS_.Models;
using Microsoft.AspNetCore.Identity;

namespace Al_Shami_AISS_.Services;

/// <summary>
/// Initializes the default ASP.NET Core Identity roles and staff users.
/// This service is intentionally separate from the controller layer so the application startup code remains focused on startup concerns
/// while the security setup remains within a dedicated service.
/// </summary>
public class IdentitySeedService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Initializes the identity seed service with the ASP.NET Core Identity managers.
    /// </summary>
    public IdentitySeedService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    /// <summary>
    /// Ensures the required application roles and default users exist in the database.
    /// </summary>
    public async Task InitializeAsync()
    {
        var roles = new[] { "Admin", "Manager" };
        foreach (var roleName in roles)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        await EnsureUserAsync(
            "admin@aiss.local",
            "Admin@123456",
            "Admin",
            "System",
            "Administrator");

        await EnsureUserAsync(
            "manager@aiss.local",
            "Manager@123456",
            "Manager",
            "System",
            "Manager");
    }

    private async Task EnsureUserAsync(string email, string password, string roleName, string firstName, string lastName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                IsActive = true,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to create {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        if (!await _userManager.IsInRoleAsync(user, roleName))
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }
    }
}
