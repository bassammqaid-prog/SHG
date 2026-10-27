using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Services;
using Al_Shami_ِAISS_.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Al_Shami_ِAISS_.Controllers;

/// <summary>
/// Public contact form controller.
/// This controller accepts visitor input, performs model validation, and forwards valid data to the service layer.
/// </summary>
public class ContactController : Controller
{
    private readonly ContactMessageService _contactMessageService;
    private readonly IEmailService _emailService;
    private readonly IActivityLogService _activityLogService;

    /// <summary>
    /// Initializes the controller using dependency injection.
    /// </summary>
    public ContactController(ContactMessageService contactMessageService, IEmailService emailService, IActivityLogService activityLogService)
    {
        _contactMessageService = contactMessageService;
        _emailService = emailService;
        _activityLogService = activityLogService;
    }

    /// <summary>
    /// Displays the public contact form.
    /// </summary>
    [HttpGet]
    public IActionResult Index()
    {
        return View(new ContactFormViewModel());
    }

    /// <summary>
    /// Processes the contact form submission.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(ContactFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }

        var contactMessage = new ContactMessage
        {
            FullName = model.FullName,
            Email = model.Email,
            Phone = model.Phone,
            Subject = model.Subject,
            Message = model.Message,
            CreatedAt = DateTime.UtcNow
        };

        await _contactMessageService.AddAsync(contactMessage);

        await _emailService.SendContactNotificationAsync(contactMessage);
        await _emailService.SendContactConfirmationAsync(contactMessage);
        await _activityLogService.LogAsync(
            "Contact Form Submitted",
            nameof(ContactMessage),
            contactMessage.Id,
            $"Contact message from {contactMessage.FullName} was submitted.");

        TempData["SuccessMessage"] = "Thank you. Your message has been sent successfully.";

        return RedirectToAction(nameof(Index));
    }
}
