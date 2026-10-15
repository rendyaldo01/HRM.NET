using System.ComponentModel.DataAnnotations;

namespace HRM.Application.DTOs.AttendanceEvents;

public sealed class RecordAttendanceEventRequest
{
    [Required]
    [StringLength(100)]
    public string EventId { get; init; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string CardUid { get; init; } = string.Empty;

    [Required]
    public DateTimeOffset OccurredAt { get; init; }
}