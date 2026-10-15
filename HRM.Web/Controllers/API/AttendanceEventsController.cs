using System.Security.Claims;
using HRM.Application.DTOs.AttendanceEvents;
using HRM.Application.Services.AttendanceEvents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Web.Controllers.Api;

[ApiController]
[Route("api/attendance/events")]
[Authorize(AuthenticationSchemes = "AttendanceDevice")]
public sealed class AttendanceEventsController : ControllerBase
{
    private readonly IAttendanceEventService _attendanceEventService;

    public AttendanceEventsController(
        IAttendanceEventService attendanceEventService)
    {
        _attendanceEventService = attendanceEventService;
    }

    [HttpPost]
    public async Task<IActionResult> Record(
        [FromBody] RecordAttendanceEventRequest request)
    {
        var deviceIdValue = User.FindFirstValue("DeviceId");

        if (!Guid.TryParse(deviceIdValue, out var deviceId))
        {
            return Unauthorized(new
            {
                message = "Invalid attendance device identity."
            });
        }

        try
        {
            var recorded = await _attendanceEventService.RecordAsync(
                deviceId,
                request);

            if (!recorded)
            {
                return Ok(new
                {
                    status = "Duplicate",
                    message = "Attendance event was already recorded."
                });
            }

            return Ok(new
            {
                status = "Recorded",
                message = "Attendance event recorded successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}