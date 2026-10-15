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
            string? searchTerm)
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

        var fromUtc = TimeZoneInfo.ConvertTimeToUtc(
            DateTime.SpecifyKind(startDate.Date, DateTimeKind.Unspecified),
            jakartaTimeZone);

        var toUtc = TimeZoneInfo.ConvertTimeToUtc(
            DateTime.SpecifyKind(endDate.Date.AddDays(1), DateTimeKind.Unspecified),
            jakartaTimeZone);

        var history = await _attendanceEventService.GetAllHistoryAsync(
            fromUtc,
            toUtc,
            searchTerm);

        ViewBag.FromDate = startDate.ToString("yyyy-MM-dd");
        ViewBag.ToDate = endDate.ToString("yyyy-MM-dd");
        ViewBag.SearchTerm = searchTerm;

        return View(history);

    }

}
