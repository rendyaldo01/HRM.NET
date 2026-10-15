using HRM.Application.DTOs.AttendanceDevices;

namespace HRM.Application.Services.AttendanceDevices;

public interface IAttendanceDeviceService
{
    Task<IReadOnlyList<AttendanceDeviceListDto>> GetAllAsync();

    Task<AttendanceDeviceRegistrationDto> RegisterAsync(
        RegisterAttendanceDeviceRequest request);

    Task<AttendanceDeviceRegistrationDto> RotateApiKeyAsync(
        Guid deviceId);
}