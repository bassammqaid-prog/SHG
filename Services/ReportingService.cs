using System.IO.Compression;
using System.Text;
using Al_Shami_ِAISS_.Areas.Admin.ViewModels.Reports;
using Al_Shami_ِAISS_.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Al_Shami_ِAISS_.Services;

/// <summary>
/// Central reporting service for operational reporting and export generation.
/// This service sits above repositories and business services, so controllers can remain thin
/// while the reporting logic, filtering, summary calculation, and export formatting stay in one place.
/// </summary>
public class ReportingService
{
    private readonly CategoryService _categoryService;
    private readonly ProductService _productService;
    private readonly ProjectService _projectService;
    private readonly ContactMessageService _contactMessageService;
    private readonly QuoteRequestService _quoteRequestService;
    private readonly IActivityLogService _activityLogService;
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReportingService"/> class.
    /// Dependency injection provides the business services and Identity manager used to assemble accurate reports.
    /// </summary>
    public ReportingService(
        CategoryService categoryService,
        ProductService productService,
        ProjectService projectService,
        ContactMessageService contactMessageService,
        QuoteRequestService quoteRequestService,
        IActivityLogService activityLogService,
        UserManager<ApplicationUser> userManager)
    {
        _categoryService = categoryService;
        _productService = productService;
        _projectService = projectService;
        _contactMessageService = contactMessageService;
        _quoteRequestService = quoteRequestService;
        _activityLogService = activityLogService;
        _userManager = userManager;
    }

    /// <summary>
    /// Builds a dashboard summary based on the requested filter criteria.
    /// </summary>
    public async Task<ReportsDashboardViewModel> GetDashboardAsync(ReportsFilterViewModel? filter = null)
    {
        var categoryFilter = filter ?? new ReportsFilterViewModel();

        var categories = await _categoryService.GetAllAsync();
        var products = await _productService.GetAllAsync();
        var projects = await _projectService.GetAllAsync();
        var contactMessages = await _contactMessageService.GetAllAsync();
        var quoteRequests = await _quoteRequestService.GetAllAsync();
        var activityLogs = await _activityLogService.GetAllAsync();
        var users = await _userManager.Users.AsNoTracking().ToListAsync();

        var filteredProducts = ApplyProductFilter(products, categoryFilter);
        var filteredProjects = ApplyProjectFilter(projects, categoryFilter);
        var filteredContactMessages = ApplyContactMessageFilter(contactMessages, categoryFilter);
        var filteredQuoteRequests = ApplyQuoteRequestFilter(quoteRequests, categoryFilter);
        var filteredActivityLogs = ApplyActivityLogFilter(activityLogs, categoryFilter);

        return new ReportsDashboardViewModel
        {
            DateFrom = categoryFilter.DateFrom,
            DateTo = categoryFilter.DateTo,
            CategoryId = categoryFilter.CategoryId,
            ProjectId = categoryFilter.ProjectId,
            UserId = categoryFilter.UserId,
            ActivityType = categoryFilter.ActivityType,
            TotalProducts = filteredProducts.Count(),
            TotalProjects = filteredProjects.Count(),
            TotalCategories = categories.Count(),
            TotalUsers = users.Count,
            TotalContactMessages = filteredContactMessages.Count(),
            TotalQuoteRequests = filteredQuoteRequests.Count(),
            TotalActivityLogs = filteredActivityLogs.Count(),
            Categories = BuildSelectList(categories, "Category", categoryFilter.CategoryId),
            Projects = BuildSelectList(projects, "Project", categoryFilter.ProjectId),
            Users = users.Select(user => new SelectListItem
            {
                Value = user.Id,
                Text = user.Email ?? user.UserName ?? "Unknown User",
                Selected = user.Id == categoryFilter.UserId
            }).ToList(),
            ActivityTypes = new List<SelectListItem>
            {
                new() { Value = string.Empty, Text = "All activity types", Selected = string.IsNullOrWhiteSpace(categoryFilter.ActivityType) },
                new() { Value = "Product Created", Text = "Product Created", Selected = categoryFilter.ActivityType == "Product Created" },
                new() { Value = "Product Updated", Text = "Product Updated", Selected = categoryFilter.ActivityType == "Product Updated" },
                new() { Value = "Product Deleted", Text = "Product Deleted", Selected = categoryFilter.ActivityType == "Product Deleted" },
                new() { Value = "Category Created", Text = "Category Created", Selected = categoryFilter.ActivityType == "Category Created" },
                new() { Value = "Project Created", Text = "Project Created", Selected = categoryFilter.ActivityType == "Project Created" },
                new() { Value = "User Login", Text = "User Login", Selected = categoryFilter.ActivityType == "User Login" },
                new() { Value = "User Logout", Text = "User Logout", Selected = categoryFilter.ActivityType == "User Logout" }
            }
        };
    }

    /// <summary>
    /// Gets the filtered product list used in report exports and summary pages.
    /// </summary>
    public async Task<IEnumerable<Product>> GetProductsReportAsync(ReportsFilterViewModel filter)
    {
        var products = await _productService.GetAllAsync();
        return ApplyProductFilter(products, filter);
    }

    /// <summary>
    /// Gets the filtered project list used in report exports and summary pages.
    /// </summary>
    public async Task<IEnumerable<Project>> GetProjectsReportAsync(ReportsFilterViewModel filter)
    {
        var projects = await _projectService.GetAllAsync();
        return ApplyProjectFilter(projects, filter);
    }

    /// <summary>
    /// Gets the filtered category list used by category reporting and export actions.
    /// </summary>
    public async Task<IEnumerable<Category>> GetCategoriesReportAsync(ReportsFilterViewModel filter)
    {
        var categories = await _categoryService.GetAllAsync();
        return ApplyCategoryFilter(categories, filter);
    }

    /// <summary>
    /// Gets the filtered contact message list used by contact report exports.
    /// </summary>
    public async Task<IEnumerable<ContactMessage>> GetContactMessagesReportAsync(ReportsFilterViewModel filter)
    {
        var messages = await _contactMessageService.GetAllAsync();
        return ApplyContactMessageFilter(messages, filter);
    }

    /// <summary>
    /// Gets the filtered quote request list used by quote exports and analytics.
    /// </summary>
    public async Task<IEnumerable<QuoteRequest>> GetQuoteRequestsReportAsync(ReportsFilterViewModel filter)
    {
        var requests = await _quoteRequestService.GetAllAsync();
        return ApplyQuoteRequestFilter(requests, filter);
    }

    /// <summary>
    /// Gets the filtered activity log list used by activity exports and operational reporting.
    /// </summary>
    public async Task<IEnumerable<ActivityLog>> GetActivityLogsReportAsync(ReportsFilterViewModel filter)
    {
        var logs = await _activityLogService.GetAllAsync();
        return ApplyActivityLogFilter(logs, filter);
    }

    /// <summary>
    /// Exports the filtered product report to an Excel workbook (.xlsx).
    /// </summary>
    public async Task<byte[]> ExportProductsToExcelAsync(ReportsFilterViewModel filter)
    {
        var rows = (await GetProductsReportAsync(filter)).ToList();
        var data = rows.Select(item => new
        {
            ProductName = item.Name,
            Category = item.Category?.Name ?? "Uncategorized",
            Brand = item.Brand ?? "-",
            Price = item.Price,
            CreatedDate = item.CreatedAt.ToString("yyyy-MM-dd")
        }).ToList();

        return BuildExcelFile("Products", data, new[] { "ProductName", "Category", "Brand", "Price", "CreatedDate" });
    }

    /// <summary>
    /// Exports the filtered project report to an Excel workbook (.xlsx).
    /// </summary>
    public async Task<byte[]> ExportProjectsToExcelAsync(ReportsFilterViewModel filter)
    {
        var rows = (await GetProjectsReportAsync(filter)).ToList();
        var data = rows.Select(project => new
        {
            ProjectName = project.Title,
            CreatedDate = project.CreatedAt.ToString("yyyy-MM-dd"),
            ImageCount = project.ProjectImages.Count,
            ClientName = project.ClientName ?? "-",
            Location = project.Location ?? "-"
        }).ToList();

        return BuildExcelFile("Projects", data, new[] { "ProjectName", "CreatedDate", "ImageCount", "ClientName", "Location" });
    }

    /// <summary>
    /// Exports the filtered category report to an Excel workbook (.xlsx).
    /// </summary>
    public async Task<byte[]> ExportCategoriesToExcelAsync(ReportsFilterViewModel filter)
    {
        var rows = (await GetCategoriesReportAsync(filter)).ToList();
        var data = rows.Select(item => new
        {
            CategoryName = item.Name,
            IsActive = item.IsActive,
            CreatedDate = item.CreatedAt.ToString("yyyy-MM-dd"),
            Description = item.Description ?? "-"
        }).ToList();

        return BuildExcelFile("Categories", data, new[] { "CategoryName", "IsActive", "CreatedDate", "Description" });
    }

    /// <summary>
    /// Exports the filtered contact message report to an Excel workbook (.xlsx).
    /// </summary>
    public async Task<byte[]> ExportContactMessagesToExcelAsync(ReportsFilterViewModel filter)
    {
        var rows = (await GetContactMessagesReportAsync(filter)).ToList();
        var data = rows.Select(item => new
        {
            FullName = item.FullName,
            Email = item.Email,
            Phone = item.Phone ?? "-",
            Subject = item.Subject,
            CreatedDate = item.CreatedAt.ToString("yyyy-MM-dd")
        }).ToList();

        return BuildExcelFile("ContactMessages", data, new[] { "FullName", "Email", "Phone", "Subject", "CreatedDate" });
    }

    /// <summary>
    /// Exports the filtered quote request report to an Excel workbook (.xlsx).
    /// </summary>
    public async Task<byte[]> ExportQuoteRequestsToExcelAsync(ReportsFilterViewModel filter)
    {
        var rows = (await GetQuoteRequestsReportAsync(filter)).ToList();
        var data = rows.Select(item => new
        {
            CustomerName = item.FullName,
            CompanyName = item.CompanyName ?? "-",
            Email = item.Email,
            Phone = item.Phone ?? "-",
            RequestDate = item.CreatedAt.ToString("yyyy-MM-dd")
        }).ToList();

        return BuildExcelFile("QuoteRequests", data, new[] { "CustomerName", "CompanyName", "Email", "Phone", "RequestDate" });
    }

    /// <summary>
    /// Exports the filtered activity log report to an Excel workbook (.xlsx).
    /// </summary>
    public async Task<byte[]> ExportActivityLogsToExcelAsync(ReportsFilterViewModel filter)
    {
        var rows = (await GetActivityLogsReportAsync(filter)).ToList();
        var data = rows.Select(item => new
        {
            User = item.UserEmail ?? item.UserId ?? "-",
            Action = item.Action,
            Entity = item.EntityName,
            Description = item.Description,
            IpAddress = item.IpAddress ?? "-",
            Date = item.CreatedAt.ToString("yyyy-MM-dd HH:mm")
        }).ToList();

        return BuildExcelFile("ActivityLogs", data, new[] { "User", "Action", "Entity", "Description", "IpAddress", "Date" });
    }

    /// <summary>
    /// Exports the filtered product data to a UTF-8 CSV file.
    /// </summary>
    public async Task<byte[]> ExportProductsToCsvAsync(ReportsFilterViewModel filter)
    {
        var rows = await GetProductsReportAsync(filter);
        var csv = new StringBuilder();
        csv.AppendLine("Product Name,Category,Brand,Price,Created Date");
        foreach (var item in rows)
        {
            csv.AppendLine($"{FormatCsv(item.Name)},{FormatCsv(item.Category?.Name ?? "Uncategorized")},{FormatCsv(item.Brand ?? "-")},{item.Price},{item.CreatedAt:yyyy-MM-dd}");
        }

        return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv.ToString())).ToArray();
    }

    /// <summary>
    /// Exports filtered project data to UTF-8 CSV format.
    /// </summary>
    public async Task<byte[]> ExportProjectsToCsvAsync(ReportsFilterViewModel filter)
    {
        var rows = await GetProjectsReportAsync(filter);
        var csv = new StringBuilder();
        csv.AppendLine("Project Name,Client Name,Location,Created Date,Image Count");
        foreach (var item in rows)
        {
            csv.AppendLine($"{FormatCsv(item.Title)},{FormatCsv(item.ClientName ?? "-")},{FormatCsv(item.Location ?? "-")},{item.CreatedAt:yyyy-MM-dd},{item.ProjectImages.Count}");
        }

        return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv.ToString())).ToArray();
    }

    /// <summary>
    /// Exports filtered contact messages to UTF-8 CSV format.
    /// </summary>
    public async Task<byte[]> ExportMessagesToCsvAsync(ReportsFilterViewModel filter)
    {
        var rows = await GetContactMessagesReportAsync(filter);
        var csv = new StringBuilder();
        csv.AppendLine("Full Name,Email,Phone,Subject,Created Date");
        foreach (var item in rows)
        {
            csv.AppendLine($"{FormatCsv(item.FullName)},{FormatCsv(item.Email)},{FormatCsv(item.Phone ?? "-")},{FormatCsv(item.Subject)},{item.CreatedAt:yyyy-MM-dd}");
        }

        return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv.ToString())).ToArray();
    }

    /// <summary>
    /// Exports filtered quote requests to UTF-8 CSV format.
    /// </summary>
    public async Task<byte[]> ExportQuotesToCsvAsync(ReportsFilterViewModel filter)
    {
        var rows = await GetQuoteRequestsReportAsync(filter);
        var csv = new StringBuilder();
        csv.AppendLine("Customer Name,Company Name,Email,Phone,Request Date");
        foreach (var item in rows)
        {
            csv.AppendLine($"{FormatCsv(item.FullName)},{FormatCsv(item.CompanyName ?? "-")},{FormatCsv(item.Email)},{FormatCsv(item.Phone ?? "-")},{item.CreatedAt:yyyy-MM-dd}");
        }

        return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv.ToString())).ToArray();
    }

    /// <summary>
    /// Exports filtered activity log records to UTF-8 CSV format.
    /// </summary>
    public async Task<byte[]> ExportActivityLogsToCsvAsync(ReportsFilterViewModel filter)
    {
        var rows = await GetActivityLogsReportAsync(filter);
        var csv = new StringBuilder();
        csv.AppendLine("User,Action,Entity,Description,IP Address,Created Date");
        foreach (var item in rows)
        {
            csv.AppendLine($"{FormatCsv(item.UserEmail ?? item.UserId ?? "-")},{FormatCsv(item.Action)},{FormatCsv(item.EntityName)},{FormatCsv(item.Description)},{FormatCsv(item.IpAddress ?? "-")},{item.CreatedAt:yyyy-MM-dd HH:mm}");
        }

        return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv.ToString())).ToArray();
    }

    /// <summary>
    /// Exports the filtered quote requests to a PDF report.
    /// </summary>
    public async Task<byte[]> ExportQuoteRequestsPdfAsync(ReportsFilterViewModel filter)
    {
        var rows = await GetQuoteRequestsReportAsync(filter);
        var data = rows.Select(item => new[]
        {
            item.FullName,
            item.CompanyName ?? "-",
            item.Email,
            item.Phone ?? "-",
            item.CreatedAt.ToString("yyyy-MM-dd")
        }).ToList();

        return BuildPdfDocument("Quote Requests Report", new[] { "Customer", "Company", "Email", "Phone", "Request Date" }, data);
    }

    /// <summary>
    /// Exports the filtered contact messages to a PDF report.
    /// </summary>
    public async Task<byte[]> ExportContactMessagesPdfAsync(ReportsFilterViewModel filter)
    {
        var rows = await GetContactMessagesReportAsync(filter);
        var data = rows.Select(item => new[]
        {
            item.FullName,
            item.Email,
            item.Phone ?? "-",
            item.Subject,
            item.CreatedAt.ToString("yyyy-MM-dd")
        }).ToList();

        return BuildPdfDocument("Contact Messages Report", new[] { "Full Name", "Email", "Phone", "Subject", "Created Date" }, data);
    }

    /// <summary>
    /// Exports the filtered activity logs to a PDF report.
    /// </summary>
    public async Task<byte[]> ExportActivityLogsPdfAsync(ReportsFilterViewModel filter)
    {
        var rows = await GetActivityLogsReportAsync(filter);
        var data = rows.Select(item => new[]
        {
            item.UserEmail ?? item.UserId ?? "-",
            item.Action,
            item.EntityName,
            item.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
            item.IpAddress ?? "-"
        }).ToList();

        return BuildPdfDocument("Activity Logs Report", new[] { "User", "Action", "Entity", "Date", "IP Address" }, data);
    }

    /// <summary>
    /// Exports a summary of product statistics to PDF.
    /// </summary>
    public async Task<byte[]> ExportProductsSummaryPdfAsync(ReportsFilterViewModel filter)
    {
        var rows = await GetProductsReportAsync(filter);
        var summary = new List<string[]>
        {
            new[] { "Metric", "Value" },
            new[] { "Total Products", rows.Count().ToString() },
            new[] { "Products by Category", rows.GroupBy(product => product.Category?.Name ?? "Uncategorized").Count().ToString() }
        };

        return BuildPdfDocument("Products Summary", summary[0], summary.Skip(1).ToList());
    }

    /// <summary>
    /// Exports a summary of project statistics to PDF.
    /// </summary>
    public async Task<byte[]> ExportProjectsSummaryPdfAsync(ReportsFilterViewModel filter)
    {
        var rows = await GetProjectsReportAsync(filter);
        var summary = new List<string[]>
        {
            new[] { "Metric", "Value" },
            new[] { "Total Projects", rows.Count().ToString() },
            new[] { "Total Project Images", rows.Sum(project => project.ProjectImages.Count).ToString() }
        };

        return BuildPdfDocument("Projects Summary", summary[0], summary.Skip(1).ToList());
    }

    private static IEnumerable<T> ApplyDateFilter<T>(IEnumerable<T> items, ReportsFilterViewModel filter, Func<T, DateTime> selector)
    {
        if (filter.DateFrom.HasValue)
        {
            items = items.Where(item => selector(item) >= filter.DateFrom.Value);
        }

        if (filter.DateTo.HasValue)
        {
            var endDate = filter.DateTo.Value.Date.AddDays(1).AddTicks(-1);
            items = items.Where(item => selector(item) <= endDate);
        }

        return items;
    }

    private static IEnumerable<Product> ApplyProductFilter(IEnumerable<Product> products, ReportsFilterViewModel filter)
    {
        var finalItems = ApplyDateFilter(products, filter, item => item.CreatedAt);

        if (filter.CategoryId.HasValue)
        {
            finalItems = finalItems.Where(item => item.CategoryId == filter.CategoryId.Value);
        }

        return finalItems.OrderByDescending(item => item.CreatedAt);
    }

    private static IEnumerable<Project> ApplyProjectFilter(IEnumerable<Project> projects, ReportsFilterViewModel filter)
    {
        var finalItems = ApplyDateFilter(projects, filter, item => item.CreatedAt);
        return finalItems.OrderByDescending(item => item.CreatedAt);
    }

    private static IEnumerable<Category> ApplyCategoryFilter(IEnumerable<Category> categories, ReportsFilterViewModel filter)
    {
        var finalItems = ApplyDateFilter(categories, filter, item => item.CreatedAt);
        return finalItems.OrderBy(item => item.Name);
    }

    private static IEnumerable<ContactMessage> ApplyContactMessageFilter(IEnumerable<ContactMessage> messages, ReportsFilterViewModel filter)
    {
        var finalItems = ApplyDateFilter(messages, filter, item => item.CreatedAt);
        return finalItems.OrderByDescending(item => item.CreatedAt);
    }

    private static IEnumerable<QuoteRequest> ApplyQuoteRequestFilter(IEnumerable<QuoteRequest> requests, ReportsFilterViewModel filter)
    {
        var finalItems = ApplyDateFilter(requests, filter, item => item.CreatedAt);
        return finalItems.OrderByDescending(item => item.CreatedAt);
    }

    private static IEnumerable<ActivityLog> ApplyActivityLogFilter(IEnumerable<ActivityLog> logs, ReportsFilterViewModel filter)
    {
        var finalItems = ApplyDateFilter(logs, filter, item => item.CreatedAt);

        if (!string.IsNullOrWhiteSpace(filter.UserId))
        {
            finalItems = finalItems.Where(item => item.UserId == filter.UserId);
        }

        if (!string.IsNullOrWhiteSpace(filter.ActivityType))
        {
            finalItems = finalItems.Where(item => item.Action == filter.ActivityType);
        }

        return finalItems.OrderByDescending(item => item.CreatedAt);
    }

    private static List<SelectListItem> BuildSelectList<T>(IEnumerable<T> items, string valueField, int? selectedValue)
        where T : class
    {
        var list = items.Select(item => new SelectListItem
        {
            Value = item.GetType().GetProperty("Id")?.GetValue(item)?.ToString() ?? string.Empty,
            Text = item.GetType().GetProperty("Name")?.GetValue(item)?.ToString() ?? item.GetType().GetProperty("Title")?.GetValue(item)?.ToString() ?? "Unknown",
            Selected = selectedValue.HasValue && item.GetType().GetProperty("Id")?.GetValue(item)?.ToString() == selectedValue.Value.ToString()
        }).ToList();

        if (valueField == "Category")
        {
            list.Insert(0, new SelectListItem { Value = string.Empty, Text = "All Categories", Selected = !selectedValue.HasValue });
        }
        else if (valueField == "Project")
        {
            list.Insert(0, new SelectListItem { Value = string.Empty, Text = "All Projects", Selected = !selectedValue.HasValue });
        }

        return list;
    }

    private static byte[] BuildExcelFile<T>(string sheetName, List<T> data, string[] headers)
    {
        var rows = new List<string[]> { headers };
        var propertyInfos = typeof(T).GetProperties();

        foreach (var item in data)
        {
            var row = new List<string>();
            foreach (var header in headers)
            {
                var property = propertyInfos.FirstOrDefault(info => string.Equals(info.Name, header, StringComparison.OrdinalIgnoreCase));
                var value = property is null ? string.Empty : property.GetValue(item)?.ToString() ?? string.Empty;
                row.Add(value);
            }

            rows.Add(row.ToArray());
        }

        using var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            AddZipEntry(archive, "[Content_Types].xml", BuildContentTypesXml());
            AddZipEntry(archive, "_rels/.rels", BuildRelXml());
            AddZipEntry(archive, "xl/workbook.xml", BuildWorkbookXml());
            AddZipEntry(archive, "xl/_rels/workbook.xml.rels", BuildWorkbookRelXml());
            AddZipEntry(archive, "xl/styles.xml", BuildStylesXml());
            AddZipEntry(archive, "xl/worksheets/sheet1.xml", BuildSheetXml(sheetName, rows));
        }

        return memoryStream.ToArray();
    }

    private static void AddZipEntry(ZipArchive archive, string entryName, string content)
    {
        var entry = archive.CreateEntry(entryName);
        using var stream = entry.Open();
        using var writer = new StreamWriter(stream, new UTF8Encoding(false));
        writer.Write(content);
    }

    private static string BuildContentTypesXml()
    {
        return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>"
            + "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">"
            + "<Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>"
            + "<Default Extension=\"xml\" ContentType=\"application/xml\"/>"
            + "<Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>"
            + "<Override PartName=\"/xl/worksheets/sheet1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>"
            + "<Override PartName=\"/xl/styles.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml\"/>"
            + "</Types>";
    }

    private static string BuildRelXml()
    {
        return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>"
            + "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">"
            + "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>"
            + "</Relationships>";
    }

    private static string BuildWorkbookXml()
    {
        return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>"
            + "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">"
            + "<sheets><sheet name=\"Report\" sheetId=\"1\" r:id=\"rId1\"/></sheets>"
            + "</workbook>";
    }

    private static string BuildWorkbookRelXml()
    {
        return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>"
            + "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">"
            + "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>"
            + "<Relationship Id=\"rId2\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles\" Target=\"styles.xml\"/>"
            + "</Relationships>";
    }

    private static string BuildStylesXml()
    {
        return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>"
            + "<styleSheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">"
            + "<fonts count=\"1\"><font><sz val=\"11\"/><name val=\"Calibri\"/></font></fonts>"
            + "<fills count=\"1\"><fill><patternFill patternType=\"none\"/></fill></fills>"
            + "<borders count=\"1\"><border><left/><right/><top/><bottom/><diagonal/></border></borders>"
            + "<cellStyleXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\"/></cellStyleXfs>"
            + "<cellXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\" xfId=\"0\"/></cellXfs>"
            + "<cellStyles count=\"1\"><cellStyle name=\"Normal\" xfId=\"0\" builtinId=\"0\"/></cellStyles>"
            + "</styleSheet>";
    }

    private static string BuildSheetXml(string sheetName, List<string[]> rows)
    {
        var builder = new StringBuilder();
        builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
        builder.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">");
        builder.Append("<dimension ref=\"A1:");
        builder.Append(ColumnName(rows[0].Length));
        builder.Append(rows.Count);
        builder.Append("\"/> <cols>");

        for (var columnIndex = 0; columnIndex < rows[0].Length; columnIndex++)
        {
            var width = 18;
            var longestValue = rows.Max(r => r.Length > columnIndex ? r[columnIndex].Length : 0);
            if (longestValue > 0)
            {
                width = Math.Min(50, Math.Max(12, longestValue + 4));
            }

            builder.Append("<col min=\"");
            builder.Append(columnIndex + 1);
            builder.Append("\" max=\"");
            builder.Append(columnIndex + 1);
            builder.Append("\" width=\"");
            builder.Append(width);
            builder.Append("\" customWidth=\"1\"/> ");
        }

        builder.Append("</cols><sheetData>");

        for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
        {
            builder.Append("<row r=\"");
            builder.Append(rowIndex + 1);
            builder.Append("\">");

            var cells = rows[rowIndex];
            for (var columnIndex = 0; columnIndex < cells.Length; columnIndex++)
            {
                var cellAddress = ColumnName(columnIndex + 1) + (rowIndex + 1);
                builder.Append("<c r=\"");
                builder.Append(cellAddress);
                builder.Append("\" t=\"inlineStr\"><is><t>");
                builder.Append(EscapeXml(cells[columnIndex]));
                builder.Append("</t></is></c>");
            }

            builder.Append("</row>");
        }

        builder.Append("</sheetData></worksheet>");
        return builder.ToString();
    }

    private static string ColumnName(int columnIndex)
    {
        var columnName = new StringBuilder();
        var value = columnIndex;

        while (value > 0)
        {
            var remainder = (value - 1) % 26;
            columnName.Insert(0, (char)('A' + remainder));
            value = (value - remainder - 1) / 26;
        }

        return columnName.ToString();
    }

    private static string EscapeXml(string value)
    {
        return string.IsNullOrEmpty(value)
            ? string.Empty
            : value.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
    }

    private static string FormatCsv(string value)
    {
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r'))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        return value;
    }

    private static byte[] BuildPdfDocument(string title, string[] columnHeaders, List<string[]> rows)
    {
        var contentLines = new List<string>
        {
            "AISS",
            title,
            $"Generated: {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC",
            string.Join(" | ", columnHeaders)
        };

        foreach (var row in rows)
        {
            contentLines.Add(string.Join(" | ", row.Select(cell => cell ?? "-")));
        }

        var contentStream = new StringBuilder();
        contentStream.Append("BT\n/F1 18 Tf\n50 760 Td\n(");
        contentStream.Append(EscapePdfText("AISS"));
        contentStream.Append(") Tj\n0 -22 Td\n(");
        contentStream.Append(EscapePdfText(title));
        contentStream.Append(") Tj\n0 -22 Td\n(");
        contentStream.Append(EscapePdfText($"Generated: {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC"));
        contentStream.Append(") Tj\n");

        foreach (var line in contentLines.Skip(3))
        {
            contentStream.Append("0 -18 Td\n(");
            contentStream.Append(EscapePdfText(line));
            contentStream.Append(") Tj\n");
        }

        contentStream.Append("ET");

        var streamBytes = Encoding.ASCII.GetBytes(contentStream.ToString());
        var objects = new List<string>
        {
            "<< /Type /Catalog /Pages 2 0 R >>",
            "<< /Type /Pages /Kids [3 0 R] /Count 1 >>",
            "<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>",
            "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>",
            $"<< /Length {streamBytes.Length} >>\nstream\n{contentStream}\nendstream"
        };

        var pdf = new StringBuilder();
        pdf.Append("%PDF-1.4\n");
        var offsets = new List<int> { 0 };

        for (var index = 0; index < objects.Count; index++)
        {
            offsets.Add(pdf.Length);
            pdf.Append(index + 1);
            pdf.Append(" 0 obj\n");
            pdf.Append(objects[index]);
            pdf.Append("\nendobj\n");
        }

        var xrefPosition = pdf.Length;
        pdf.Append("xref\n0 ");
        pdf.Append(objects.Count + 1);
        pdf.Append("\n0000000000 65535 f \n");

        foreach (var offset in offsets.Skip(1))
        {
            pdf.Append(offset.ToString("D10"));
            pdf.Append(" 00000 n \n");
        }

        pdf.Append("trailer\n");
        pdf.Append("<< /Size ");
        pdf.Append(objects.Count + 1);
        pdf.Append(" /Root 1 0 R >>\nstartxref\n");
        pdf.Append(xrefPosition);
        pdf.Append("\n%%EOF");

        return Encoding.ASCII.GetBytes(pdf.ToString());
    }

    private static string EscapePdfText(string value)
    {
        return (value ?? string.Empty)
            .Replace("\\", "\\\\")
            .Replace("(", "\\(")
            .Replace(")", "\\)");
    }
}
