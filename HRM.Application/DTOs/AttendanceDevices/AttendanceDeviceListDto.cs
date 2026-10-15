namespace HRM.Application.DTOs.AttendanceDevices;

public class AttendanceDeviceListDto
{
    public Guid Id { get; set; }

    public string DeviceCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}