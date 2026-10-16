using HRM.Application.DTOs.AttendanceEvents;
using HRM.Application.Interfaces.Repositories;
using HRM.Domain.Entities;
using HRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRM.Infrastructure.Repositories;

public sealed class AttendanceEventRepository : IAttendanceEventRepository
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

    public async Task<IReadOnlyList<AttendanceEventHistoryDto>> GetHistoryByEmployeeAsync(
        Guid employeeId,
        DateTime fromUtc,
        DateTime toUtc)
    {
        return await (
        from attendance in _context.AttendanceEvents.AsNoTracking()
        join card in _context.RfidCards.AsNoTracking()
        on attendance.CardUid equals card.CardUid
        join employee in _context.Employees.AsNoTracking()
        on card.EmployeeId equals employee.Id
        where employee.Id == employeeId
        && attendance.OccurredAtUtc >= fromUtc
        && attendance.OccurredAtUtc < toUtc
        orderby attendance.OccurredAtUtc descending
        select new AttendanceEventHistoryDto
        {
            Id = attendance.Id,
            EmployeeNumber = employee.EmployeeNumber,
            EmployeeName = (employee.FirstName + " " + employee.LastName).Trim(),
            CardUid = attendance.CardUid,
            OccurredAtUtc = attendance.OccurredAtUtc,
            ReceivedAtUtc = attendance.ReceivedAtUtc
        })
        .ToListAsync();
    }


    public async Task<IReadOnlyList<AttendanceEventHistoryDto>> GetAllHistoryAsync(
        DateTime fromUtc,
        DateTime toUtc,
        string? searchTerm,
        int page,
        int pageSize)
    {
        var query =
            from attendance in _context.AttendanceEvents.AsNoTracking()
            join card in _context.RfidCards.AsNoTracking()
                on attendance.CardUid equals card.CardUid
            join employee in _context.Employees.AsNoTracking()
                on card.EmployeeId equals employee.Id
            where attendance.OccurredAtUtc >= fromUtc
                && attendance.OccurredAtUtc < toUtc
            select new
            {
                Attendance = attendance,
                Employee = employee
            };

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim();

            query = query.Where(x =>
                x.Employee.EmployeeNumber.Contains(term) ||
                (x.Employee.FirstName + " " + x.Employee.LastName)
                    .Contains(term));
        }

        return await query
            .OrderByDescending(x => x.Attendance.OccurredAtUtc)
            .ThenByDescending(x => x.Attendance.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AttendanceEventHistoryDto
            {
                Id = x.Attendance.Id,
                EmployeeNumber = x.Employee.EmployeeNumber,
                EmployeeName = (
                    x.Employee.FirstName + " " + x.Employee.LastName
                ).Trim(),
                CardUid = x.Attendance.CardUid,
                OccurredAtUtc = x.Attendance.OccurredAtUtc,
                ReceivedAtUtc = x.Attendance.ReceivedAtUtc
            })
            .ToListAsync();
    }


    public async Task<int> CountAllHistoryAsync(
        DateTime fromUtc,
        DateTime toUtc,
        string? searchTerm)
    {
        var query =
            from attendance in _context.AttendanceEvents.AsNoTracking()
            join card in _context.RfidCards.AsNoTracking()
                on attendance.CardUid equals card.CardUid
            join employee in _context.Employees.AsNoTracking()
                on card.EmployeeId equals employee.Id
            where attendance.OccurredAtUtc >= fromUtc
                && attendance.OccurredAtUtc < toUtc
            select new
            {
                Employee = employee
            };

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim();

            query = query.Where(x =>
                x.Employee.EmployeeNumber.Contains(term) ||
                (x.Employee.FirstName + " " + x.Employee.LastName)
                    .Contains(term));
        }

        return await query.CountAsync();
    }


}