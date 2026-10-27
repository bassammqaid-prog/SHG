using Al_Shami_ِAISS_.Areas.Admin.ViewModels;
using Al_Shami_ِAISS_.Models;
using Al_Shami_ِAISS_.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Al_Shami_ِAISS_.Areas.Admin.Controllers;

/// <summary>
/// Handles the quote request admin module.
/// The controller orchestrates service calls and prepares ViewModels for the UI.
/// </summary>
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class QuoteRequestsController : Controller
{
    private readonly QuoteRequestService _quoteRequestService;

    /// <summary>
    /// Initializes the controller with a quote request service.
    /// </summary>
    public QuoteRequestsController(QuoteRequestService quoteRequestService)
    {
        _quoteRequestService = quoteRequestService;
    }

    /// <summary>
    /// Displays all quote requests.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var requests = await _quoteRequestService.GetAllAsync();
        var viewModel = requests.Select(q => new QuoteRequestListItemViewModel
        {
            Id = q.Id,
            FullName = q.FullName,
            CompanyName = q.CompanyName,
            Email = q.Email,
            CreatedAt = q.CreatedAt
        }).ToList();

        return View(viewModel);
    }

    /// <summary>
    /// Displays a quote request detail record.
    /// </summary>
    public async Task<IActionResult> Details(int id)
    {
        var quoteRequest = await _quoteRequestService.GetByIdAsync(id);
        if (quoteRequest == null)
        {
            return NotFound();
        }

        var viewModel = new QuoteRequestDetailsViewModel
        {
            Id = quoteRequest.Id,
            FullName = quoteRequest.FullName,
            CompanyName = quoteRequest.CompanyName,
            Email = quoteRequest.Email,
            Phone = quoteRequest.Phone,
            Message = quoteRequest.Message,
            CreatedAt = quoteRequest.CreatedAt
        };

        return View(viewModel);
    }

    /// <summary>
    /// Displays the create form.
    /// </summary>
    public IActionResult Create()
    {
        return View(new QuoteRequestCreateViewModel());
    }

    /// <summary>
    /// Processes creation of a quote request.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(QuoteRequestCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
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
        TempData["SuccessMessage"] = "Quote request created successfully.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays the edit form.
    /// </summary>
    public async Task<IActionResult> Edit(int id)
    {
        var quoteRequest = await _quoteRequestService.GetByIdAsync(id);
        if (quoteRequest == null)
        {
            return NotFound();
        }

        var viewModel = new QuoteRequestEditViewModel
        {
            Id = quoteRequest.Id,
            FullName = quoteRequest.FullName,
            CompanyName = quoteRequest.CompanyName,
            Email = quoteRequest.Email,
            Phone = quoteRequest.Phone,
            Message = quoteRequest.Message
        };

        return View(viewModel);
    }

    /// <summary>
    /// Processes quote request update.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, QuoteRequestEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var quoteRequest = await _quoteRequestService.GetByIdAsync(id);
        if (quoteRequest == null)
        {
            return NotFound();
        }

        quoteRequest.FullName = model.FullName;
        quoteRequest.CompanyName = model.CompanyName;
        quoteRequest.Email = model.Email;
        quoteRequest.Phone = model.Phone;
        quoteRequest.Message = model.Message;

        await _quoteRequestService.UpdateAsync(quoteRequest);
        TempData["SuccessMessage"] = "Quote request updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays delete confirmation.
    /// </summary>
    public async Task<IActionResult> Delete(int id)
    {
        var quoteRequest = await _quoteRequestService.GetByIdAsync(id);
        if (quoteRequest == null)
        {
            return NotFound();
        }

        var viewModel = new QuoteRequestDeleteViewModel
        {
            Id = quoteRequest.Id,
            FullName = quoteRequest.FullName,
            CompanyName = quoteRequest.CompanyName,
            Email = quoteRequest.Email
        };

        return View(viewModel);
    }

    /// <summary>
    /// Processes quote request deletion.
    /// </summary>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _quoteRequestService.DeleteAsync(id);
        TempData["SuccessMessage"] = "Quote request deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
