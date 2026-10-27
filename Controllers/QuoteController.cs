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
    private readonly IEmailService _emailService;
    private readonly IActivityLogService _activityLogService;

    /// <summary>
    /// Initializes the controller through dependency injection.
    /// </summary>
    public QuoteController(QuoteRequestService quoteRequestService, IEmailService emailService, IActivityLogService activityLogService)
    {
        _quoteRequestService = quoteRequestService;
        _emailService = emailService;
        _activityLogService = activityLogService;
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

        await _emailService.SendQuoteRequestNotificationAsync(quoteRequest);
        await _emailService.SendQuoteRequestConfirmationAsync(quoteRequest);
        await _activityLogService.LogAsync(
            "Quote Request Submitted",
            nameof(QuoteRequest),
            quoteRequest.Id,
            $"Quote request from {quoteRequest.FullName} was submitted.");

        TempData["SuccessMessage"] = "Your quote request has been submitted successfully.";

        return RedirectToAction(nameof(Index));
    }
}
