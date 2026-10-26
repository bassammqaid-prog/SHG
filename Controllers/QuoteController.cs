using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Services;
using Al_Shami_ِAISS_.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Al_Shami_ِAISS_.Controllers;

/// <summary>
/// Public quote request controller.
/// It accepts visitor requests for pricing and forwards valid submissions to the business service layer.
/// </summary>
public class QuoteController : Controller
{
    private readonly QuoteRequestService _quoteRequestService;

    /// <summary>
    /// Initializes the controller through dependency injection.
    /// </summary>
    public QuoteController(QuoteRequestService quoteRequestService)
    {
        _quoteRequestService = quoteRequestService;
    }

    /// <summary>
    /// Displays the quote request form.
    /// </summary>
    [HttpGet]
    public IActionResult Index()
    {
        return View(new QuoteRequestViewModel());
    }

    /// <summary>
    /// Processes a submitted quote request.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(QuoteRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }

        var quoteRequest = new QuoteRequest
        {
            FullName = model.FullName,
            CompanyName = model.CompanyName,
            Email = model.Email,
            Phone = model.Phone,
            Message = model.Message,
            CreatedAt = DateTime.UtcNow
        };

        await _quoteRequestService.AddAsync(quoteRequest);
        TempData["SuccessMessage"] = "Your quote request has been submitted successfully.";

        return RedirectToAction(nameof(Index));
    }
}
