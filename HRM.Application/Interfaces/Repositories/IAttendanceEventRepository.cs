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
}