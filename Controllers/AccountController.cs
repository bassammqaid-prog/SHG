using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Services;
using Al_Shami_ِAISS_.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Al_Shami_ِAISS_.Controllers;

/// <summary>
/// Handles the public authentication flow for staff and administrators.
/// The controller uses ASP.NET Core Identity components such as SignInManager and UserManager instead of manually manipulating database records.
/// </summary>
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IActivityLogService _activityLogService;

    /// <summary>
    /// Initializes the controller with ASP.NET Core Identity services.
    /// </summary>
    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IActivityLogService activityLogService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _activityLogService = activityLogService;
    }

    /// <summary>
    /// Displays the login page.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    /// <summary>
    /// Authenticates the user and signs them into the application using a secure cookie.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || !user.IsActive)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            await _activityLogService.LogLoginAsync(user.Id, user.Email ?? user.UserName);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            if (await _userManager.IsInRoleAsync(user, "Manager"))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }

    /// <summary>
    /// Signs the user out and clears their identity cookie.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is not null)
        {
            await _activityLogService.LogLogoutAsync(user.Id, user.Email ?? user.UserName);
        }

        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home", new { area = "" });
    }

    /// <summary>
    /// Displays a user-friendly message when access is denied.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
