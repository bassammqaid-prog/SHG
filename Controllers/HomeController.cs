using Microsoft.AspNetCore.Mvc;

namespace Al_Shami_ِAISS_.Controllers;

/// <summary>
/// Default entry controller for the application.
/// This keeps the default routed URL working while the platform is still under development.
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// Returns a simple landing message for the default homepage.
    /// </summary>
    /// <returns>A plain text response.</returns>
    public IActionResult Index()
    {
        return Content("Al-Shami Integrated Solutions & Supply (AISS) - architecture is ready for the first application phase.");
    }
}
