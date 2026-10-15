
using HRM.Application.Interfaces.Repositories;
using HRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRM.Infrastructure.Persistence.Repositories;

public class AttendanceDeviceRepository : IAttendanceDeviceRepository
{
    private readonly ApplicationDbContext _context;

    public AttendanceDeviceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AttendanceDevice?> GetByIdAsync(Guid id)
    {
        return await _context.AttendanceDevices
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<AttendanceDevice?> GetByDeviceCodeAsync(
        string deviceCode)
    {
        var normalizedCode = deviceCode.Trim().ToUpperInvariant();

        return await _context.AttendanceDevices
            .FirstOrDefaultAsync(x => x.DeviceCode == normalizedCode);
    }

    public async Task<IReadOnlyList<AttendanceDevice>> GetAllAsync()
    {
        return await _context.AttendanceDevices
            .AsNoTracking()
            .OrderBy(x => x.DeviceCode)
            .ToListAsync();
    }

    public async Task AddAsync(AttendanceDevice device)
    {
        await _context.AttendanceDevices.AddAsync(device);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AttendanceDevice device)
    {
        _context.AttendanceDevices.Update(device);
        await _context.SaveChangesAsync();
    }

    public async Task<AttendanceDevice?> GetByApiKeyHashAsync(
        string apiKeyHash)
    {
        return await _context.AttendanceDevices
            .FirstOrDefaultAsync(x => x.ApiKeyHash == apiKeyHash);
    }

}
