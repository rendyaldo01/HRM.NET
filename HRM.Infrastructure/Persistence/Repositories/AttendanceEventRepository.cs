using HRM.Application.Interfaces.Repositories;
using HRM.Domain.Entities;
using HRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRM.Infrastructure.Repositories;

public sealed class AttendanceEventRepository
    : IAttendanceEventRepository
{
    private readonly ApplicationDbContext _context;

    public AttendanceEventRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AttendanceEvent?> GetByDeviceAndEventIdAsync(
        Guid deviceId,
        string eventId)
    {
        return await _context.AttendanceEvents
            .FirstOrDefaultAsync(x =>
                x.DeviceId == deviceId &&
                x.EventId == eventId);
    }

    public async Task<bool> AddAsync(
        AttendanceEvent attendanceEvent)
    {
        await _context.AttendanceEvents.AddAsync(
            attendanceEvent);

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            _context.Entry(attendanceEvent).State =
                EntityState.Detached;

            var alreadyExists =
                await _context.AttendanceEvents
                    .AsNoTracking()
                    .AnyAsync(x =>
                        x.DeviceId == attendanceEvent.DeviceId &&
                        x.EventId == attendanceEvent.EventId);

            if (alreadyExists)
            {
                return false;
            }

            throw;
        }
    }

    public async Task<bool> ExistsAsync(
        Guid deviceId,
        string eventId)
    {
        return await _context.AttendanceEvents
            .AnyAsync(x =>
                x.DeviceId == deviceId &&
                x.EventId == eventId);
    }
}