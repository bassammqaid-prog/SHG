using Al_Shami_AISS_.Areas.Admin.ViewModels;
using Al_Shami_AISS_.Models;
using Al_Shami_AISS_.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Al_Shami_AISS_.Areas.Admin.Controllers;

/// <summary>
/// Handles the contact message module in the admin area.
/// It reads submissions from the database and provides create/edit/delete operations.
/// </summary>
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ContactMessagesController : Controller
{
    private readonly ContactMessageService _contactMessageService;

    /// <summary>
    /// Initializes the controller with the contact message service.
    /// </summary>
    public ContactMessagesController(ContactMessageService contactMessageService)
    {
        _contactMessageService = contactMessageService;
    }

    /// <summary>
    /// Lists all stored contact messages.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var messages = await _contactMessageService.GetAllAsync();
        var viewModel = messages.Select(m => new ContactMessageListItemViewModel
        {
            Id = m.Id,
            FullName = m.FullName,
            Email = m.Email,
            Subject = m.Subject,
            CreatedAt = m.CreatedAt
        }).ToList();

        return View(viewModel);
    }

    /// <summary>
    /// Displays a single contact message.
    /// </summary>
    public async Task<IActionResult> Details(int id)
    {
        var message = await _contactMessageService.GetByIdAsync(id);
        if (message == null)
        {
            return NotFound();
        }

        var viewModel = new ContactMessageDetailsViewModel
        {
            Id = message.Id,
            FullName = message.FullName,
            Email = message.Email,
            Phone = message.Phone,
            Subject = message.Subject,
            Message = message.Message,
            CreatedAt = message.CreatedAt
        };

        return View(viewModel);
    }

    /// <summary>
    /// Displays the create form.
    /// </summary>
    public IActionResult Create()
    {
        return View(new ContactMessageCreateViewModel());
    }

    /// <summary>
    /// Processes contact message creation.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContactMessageCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var message = new ContactMessage
        {
            FullName = model.FullName,
            Email = model.Email,
            Phone = model.Phone,
            Subject = model.Subject,
            Message = model.Message,
            CreatedAt = DateTime.UtcNow
        };

        await _contactMessageService.AddAsync(message);
        TempData["SuccessMessage"] = "Contact message saved successfully.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays the edit form.
    /// </summary>
    public async Task<IActionResult> Edit(int id)
    {
        var message = await _contactMessageService.GetByIdAsync(id);
        if (message == null)
        {
            return NotFound();
        }

        var viewModel = new ContactMessageEditViewModel
        {
            Id = message.Id,
            FullName = message.FullName,
            Email = message.Email,
            Phone = message.Phone,
            Subject = message.Subject,
            Message = message.Message
        };

        return View(viewModel);
    }

    /// <summary>
    /// Processes contact message updates.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ContactMessageEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var message = await _contactMessageService.GetByIdAsync(id);
        if (message == null)
        {
            return NotFound();
        }

        message.FullName = model.FullName;
        message.Email = model.Email;
        message.Phone = model.Phone;
        message.Subject = model.Subject;
        message.Message = model.Message;

        await _contactMessageService.UpdateAsync(message);
        TempData["SuccessMessage"] = "Contact message updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays delete confirmation.
    /// </summary>
    public async Task<IActionResult> Delete(int id)
    {
        var message = await _contactMessageService.GetByIdAsync(id);
        if (message == null)
        {
            return NotFound();
        }

        var viewModel = new ContactMessageDeleteViewModel
        {
            Id = message.Id,
            FullName = message.FullName,
            Subject = message.Subject,
            Email = message.Email
        };

        return View(viewModel);
    }

    /// <summary>
    /// Processes contact message deletion.
    /// </summary>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _contactMessageService.DeleteAsync(id);
        TempData["SuccessMessage"] = "Contact message deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
