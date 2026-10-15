namespace HRM.Application.DTOs.AttendanceEvents;

public sealed class AttendanceEventHistoryDto
{
    public Guid Id { get; init; }

    public string EmployeeNumber { get; init; } = string.Empty;

    public string EmployeeName { get; init; } = string.Empty;

    public string CardUid { get; init; } = string.Empty;

    public DateTime OccurredAtUtc { get; init; }

    public DateTime ReceivedAtUtc { get; init; }
}