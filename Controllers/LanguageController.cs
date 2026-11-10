using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Al_Shami_AISS_.Controllers;

/// <summary>
/// Handles the user-selected culture and persists it in a cookie for subsequent requests.
/// This controller is the bridge between the UI language switcher and ASP.NET Core localization.
/// </summary>
public class LanguageController : Controller
{
    /// <summary>
    /// Sets the active culture and redirects the user back to the current page.
    /// </summary>
    /// <param name="culture">The culture to apply, such as ar-YE or en-US.</param>
    /// <param name="returnUrl">The page the user was visiting before changing languages.</param>
    /// <returns>A redirect back to the page the user was viewing.</returns>
    public IActionResult SetLanguage(string culture, string? returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(culture))
        {
            var supportedCulures = new[] { "ar-YE", "en-US" };
            if (supportedCulures.Contains(culture, StringComparer.OrdinalIgnoreCase))
            {
                var requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
                var currentCulture = requestCultureFeature?.RequestCulture.Culture ?? CultureInfo.CurrentCulture;

                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1),
                        IsEssential = true,
                        SameSite = SameSiteMode.Lax
                    });

                CultureInfo.CurrentCulture = new CultureInfo(culture);
                CultureInfo.CurrentUICulture = new CultureInfo(culture);
            }
        }

        return LocalRedirect(string.IsNullOrEmpty(returnUrl) ? Url.Content("~/")! : returnUrl);
    }
}
