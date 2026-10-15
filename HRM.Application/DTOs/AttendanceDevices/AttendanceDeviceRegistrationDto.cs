
namespace HRM.Application.DTOs.AttendanceDevices;

public class AttendanceDeviceRegistrationDto
{
    public Guid Id { get; set; }

    public string DeviceCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }
}
