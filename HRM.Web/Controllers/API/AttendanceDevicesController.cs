using HRM.Application.DTOs.AttendanceDevices;
using HRM.Application.Services.AttendanceDevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Web.Controllers.Api;

[ApiController]
[Route("api/attendance-devices")]
[Authorize(Roles = "Admin HR")]
public class AttendanceDevicesController : ControllerBase
{
    private readonly IAttendanceDeviceService _deviceService;

    public AttendanceDevicesController(
        IAttendanceDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpPost]
    public async Task<IActionResult> Register(
        [FromBody] RegisterAttendanceDeviceRequest request)
    {
        try
        {
            var device = await _deviceService.RegisterAsync(request);

            return StatusCode(StatusCodes.Status201Created, device);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("{deviceId:guid}/rotate-api-key")]
    public async Task<IActionResult> RotateApiKey(Guid deviceId)
    {
        try
        {
            var result = await _deviceService.RotateApiKeyAsync(deviceId);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
