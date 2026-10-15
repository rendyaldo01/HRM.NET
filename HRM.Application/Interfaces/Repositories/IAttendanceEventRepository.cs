using HRM.Application.DTOs.AttendanceEvents;
using HRM.Domain.Entities;

namespace HRM.Application.Interfaces.Repositories;

public interface IAttendanceEventRepository
{
    Task<AttendanceEvent?> GetByDeviceAndEventIdAsync(
    Guid deviceId,
    string eventId);

    Task<bool> AddAsync(
        AttendanceEvent attendanceEvent);

    Task<bool> ExistsAsync(
        Guid deviceId,
        string eventId);

    Task<IReadOnlyList<AttendanceEventHistoryDto>> GetHistoryByEmployeeAsync(
        Guid employeeId,
        DateTime fromUtc,
        DateTime toUtc);

    Task<IReadOnlyList<AttendanceEventHistoryDto>> GetAllHistoryAsync(
        DateTime fromUtc,
        DateTime toUtc);

}