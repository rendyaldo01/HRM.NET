using HRM.Application.Services.AttendanceEvents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Web.Controllers;

[Authorize(Roles = "Admin HR")]
public class AttendanceHistoryController : Controller
{
    private readonly IAttendanceEventService _attendanceEventService;

    public AttendanceHistoryController(
        IAttendanceEventService attendanceEventService)
    {
        _attendanceEventService = attendanceEventService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        DateTime? fromDate,
        DateTime? toDate,
        string? searchTerm,
        int page = 1,
        int pageSize = 10)
    {
        var jakartaTimeZone = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows()
                ? "SE Asia Standard Time"
                : "Asia/Jakarta");

        var todayJakarta = TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            jakartaTimeZone).Date;

        var startDate = fromDate ?? todayJakarta;
        var endDate = toDate ?? todayJakarta;

        if (startDate > endDate)
        {
            ModelState.AddModelError(
                string.Empty,
                "Tanggal mulai tidak boleh lebih besar dari tanggal akhir.");

            return BadRequest(ModelState);
        }

        // Normalize pagination parameters.
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        // Convert Jakarta date range to UTC.
        var fromUtc = TimeZoneInfo.ConvertTimeToUtc(
            DateTime.SpecifyKind(
                startDate.Date,
                DateTimeKind.Unspecified),
            jakartaTimeZone);

        var toUtc = TimeZoneInfo.ConvertTimeToUtc(
            DateTime.SpecifyKind(
                endDate.Date.AddDays(1),
                DateTimeKind.Unspecified),
            jakartaTimeZone);

        // Count matching records before pagination.
        var totalRecords =
            await _attendanceEventService.CountAllHistoryAsync(
                fromUtc,
                toUtc,
                searchTerm);

        var totalPages = (int)Math.Ceiling(
            totalRecords / (double)pageSize);

        // Avoid requesting a page beyond the available range.
        if (totalPages > 0 && page > totalPages)
        {
            page = totalPages;
        }

        // Fetch only the requested page.
        var history = await _attendanceEventService.GetAllHistoryAsync(
            fromUtc,
            toUtc,
            searchTerm,
            page,
            pageSize);

        // Preserve filter and pagination values for the view.
        ViewBag.FromDate = startDate.ToString("yyyy-MM-dd");
        ViewBag.ToDate = endDate.ToString("yyyy-MM-dd");
        ViewBag.SearchTerm = searchTerm;
        ViewBag.CurrentPage = page;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalRecords = totalRecords;
        ViewBag.TotalPages = totalPages;

        return View(history);
    }
}