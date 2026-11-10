using Al_Shami_AISS_.Areas.Admin.ViewModels.Reports;
using Al_Shami_AISS_.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Al_Shami_AISS_.Areas.Admin.Controllers;

/// <summary>
/// Provides the reporting and export dashboard for administrators and managers.
/// The controller keeps routing and HTTP concerns separate from the business logic, while the
/// <see cref="ReportingService"/> performs the actual data gathering and file generation.
/// </summary>
[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public class ReportsController : Controller
{
    private readonly ReportingService _reportingService;

    /// <summary>
    /// Initializes the controller using dependency injection.
    /// </summary>
    /// <param name="reportingService">The service that prepares report summaries and generated export files.</param>
    public ReportsController(ReportingService reportingService)
    {
        _reportingService = reportingService;
    }

    /// <summary>
    /// Displays the report dashboard and summary counters for the business.
    /// </summary>
    /// <param name="filter">Optional report filters used to scope the totals and dashboard display.</param>
    public async Task<IActionResult> Index(ReportsFilterViewModel filter)
    {
        var model = await _reportingService.GetDashboardAsync(filter);
        return View(model);
    }

    /// <summary>
    /// Exports the filtered product list to an Excel workbook.
    /// </summary>
    public async Task<IActionResult> ExportProductsToExcel(ReportsFilterViewModel filter)
    {
        var bytes = await _reportingService.ExportProductsToExcelAsync(filter);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProductsReport.xlsx");
    }

    /// <summary>
    /// Exports the filtered project list to an Excel workbook.
    /// </summary>
    public async Task<IActionResult> ExportProjectsToExcel(ReportsFilterViewModel filter)
    {
        var bytes = await _reportingService.ExportProjectsToExcelAsync(filter);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProjectsReport.xlsx");
    }

    /// <summary>
    /// Exports the filtered category list to an Excel workbook.
    /// </summary>
    public async Task<IActionResult> ExportCategoriesToExcel(ReportsFilterViewModel filter)
    {
        var bytes = await _reportingService.ExportCategoriesToExcelAsync(filter);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CategoriesReport.xlsx");
    }

    /// <summary>
    /// Exports the filtered contact message list to an Excel workbook.
    /// </summary>
    public async Task<IActionResult> ExportContactMessagesToExcel(ReportsFilterViewModel filter)
    {
        var bytes = await _reportingService.ExportContactMessagesToExcelAsync(filter);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ContactMessagesReport.xlsx");
    }

    /// <summary>
    /// Exports the filtered quote request list to an Excel workbook.
    /// </summary>
    public async Task<IActionResult> ExportQuoteRequestsToExcel(ReportsFilterViewModel filter)
    {
        var bytes = await _reportingService.ExportQuoteRequestsToExcelAsync(filter);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "QuoteRequestsReport.xlsx");
    }

    /// <summary>
    /// Exports the filtered activity log list to an Excel workbook.
    /// </summary>
    public async Task<IActionResult> ExportActivityLogsToExcel(ReportsFilterViewModel filter)
    {
        var bytes = await _reportingService.ExportActivityLogsToExcelAsync(filter);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ActivityLogsReport.xlsx");
    }

    /// <summary>
    /// Exports the filtered product list to a CSV file.
    /// </summary>
    public async Task<IActionResult> ExportProductsToCsv(ReportsFilterViewModel filter)
    {
        var bytes = await _reportingService.ExportProductsToCsvAsync(filter);
        return File(bytes, "text/csv;charset=utf-8", "ProductsReport.csv");
    }

    /// <summary>
    /// Exports the filtered project list to a CSV file.
    /// </summary>
    public async Task<IActionResult> ExportProjectsToCsv(ReportsFilterViewModel filter)
    {
        var bytes = await _reportingService.ExportProjectsToCsvAsync(filter);
        return File(bytes, "text/csv;charset=utf-8", "ProjectsReport.csv");
    }

    /// <summary>
    /// Exports the filtered contact message list to a CSV file.
    /// </summary>
    public async Task<IActionResult> ExportMessagesToCsv(ReportsFilterViewModel filter)
    {
        var bytes = await _reportingService.ExportMessagesToCsvAsync(filter);
        return File(bytes, "text/csv;charset=utf-8", "ContactMessagesReport.csv");
    }

    /// <summary>
    /// Exports the filtered quote request list to a CSV file.
    /// </summary>
    public async Task<IActionResult> ExportQuotesToCsv(ReportsFilterViewModel filter)
    {
        var bytes = await _reportingService.ExportQuotesToCsvAsync(filter);
        return File(bytes, "text/csv;charset=utf-8", "QuoteRequestsReport.csv");
    }

    /// <summary>
    /// Exports the filtered activity log list to a CSV file.
    /// </summary>
    public async Task<IActionResult> ExportActivityLogsToCsv(ReportsFilterViewModel filter)
    {
        var bytes = await _reportingService.ExportActivityLogsToCsvAsync(filter);
        return File(bytes, "text/csv;charset=utf-8", "ActivityLogsReport.csv");
    }

    /// <summary>
    /// Exports the filtered quote request list to PDF.
    /// </summary>
    public async Task<IActionResult> ExportQuoteRequestsToPdf(ReportsFilterViewModel filter)
    {
        var bytes = await _reportingService.ExportQuoteRequestsPdfAsync(filter);
        return File(bytes, "application/pdf", "QuoteRequestsReport.pdf");
    }

    /// <summary>
    /// Exports the filtered contact message list to PDF.
    /// </summary>
    public async Task<IActionResult> ExportContactMessagesToPdf(ReportsFilterViewModel filter)
    {
        var bytes = await _reportingService.ExportContactMessagesPdfAsync(filter);
        return File(bytes, "application/pdf", "ContactMessagesReport.pdf");
    }

    /// <summary>
    /// Exports the filtered activity log list to PDF.
    /// </summary>
    public async Task<IActionResult> ExportActivityLogsToPdf(ReportsFilterViewModel filter)
    {
        var bytes = await _reportingService.ExportActivityLogsPdfAsync(filter);
        return File(bytes, "application/pdf", "ActivityLogsReport.pdf");
    }

    /// <summary>
    /// Exports the filtered product summary data to PDF.
    /// </summary>
    public async Task<IActionResult> ExportProductsSummaryToPdf(ReportsFilterViewModel filter)
    {
        var bytes = await _reportingService.ExportProductsSummaryPdfAsync(filter);
        return File(bytes, "application/pdf", "ProductsSummaryReport.pdf");
    }

    /// <summary>
    /// Exports the filtered project summary data to PDF.
    /// </summary>
    public async Task<IActionResult> ExportProjectsSummaryToPdf(ReportsFilterViewModel filter)
    {
        var bytes = await _reportingService.ExportProjectsSummaryPdfAsync(filter);
        return File(bytes, "application/pdf", "ProjectsSummaryReport.pdf");
    }
}
