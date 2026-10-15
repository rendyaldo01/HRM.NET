using HRM.Application.Services.AttendanceDevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Web.Controllers;

[Authorize(Roles = "Admin HR")]
public class AttendanceDevicesController : Controller
{
    private readonly IAttendanceDeviceService _attendanceDeviceService;

    public AttendanceDevicesController(
        IAttendanceDeviceService attendanceDeviceService)
    {
        _attendanceDeviceService = attendanceDeviceService;
    }

    public async Task<IActionResult> Index()
    {
        var devices = await _attendanceDeviceService.GetAllAsync();

        return View(devices);
    }
}