
using HRM.Application.Interfaces.Repositories;
using HRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRM.Infrastructure.Persistence.Repositories;

public class RfidCardRepository : IRfidCardRepository
{
    private readonly ApplicationDbContext _context;

    public RfidCardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RfidCard?> GetByIdAsync(Guid id)
    {
        return await _context.RfidCards
            .Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<RfidCard?> GetByCardUidAsync(string cardUid)
    {
        var normalizedUid = cardUid.Trim().ToUpperInvariant();

        return await _context.RfidCards
            .Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.CardUid == normalizedUid);
    }

    public async Task<RfidCard?> GetActiveByEmployeeIdAsync(
        Guid employeeId)
    {
        return await _context.RfidCards
            .FirstOrDefaultAsync(
                x => x.EmployeeId == employeeId && x.IsActive);
    }

    public async Task<IReadOnlyList<RfidCard>> GetAllByEmployeeIdAsync(
        Guid employeeId)
    {
        return await _context.RfidCards
            .AsNoTracking()
            .Where(x => x.EmployeeId == employeeId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<RfidCard>> GetAllAsync()
    {
        return await _context.RfidCards
            .AsNoTracking()
            .Include(x => x.Employee)
            .OrderBy(x => x.CardUid)
            .ToListAsync();
    }

    public async Task AddAsync(RfidCard rfidCard)
    {
        await _context.RfidCards.AddAsync(rfidCard);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(RfidCard rfidCard)
    {
        _context.RfidCards.Update(rfidCard);
        await _context.SaveChangesAsync();
    }
}
