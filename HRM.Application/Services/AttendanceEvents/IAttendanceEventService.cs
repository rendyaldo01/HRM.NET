using HRM.Application.DTOs.AttendanceEvents;

namespace HRM.Application.Services.AttendanceEvents;

public interface IAttendanceEventService
{
    Task<bool> RecordAsync(
        Guid deviceId,
        RecordAttendanceEventRequest request);
}