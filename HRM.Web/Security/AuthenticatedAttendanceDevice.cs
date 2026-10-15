namespace HRM.Web.Security;

public sealed class AuthenticatedAttendanceDevice
{
    public Guid DeviceId { get; init; }

    public string DeviceCode { get; init; } = string.Empty;
}
