using HRM.Application.Interfaces.Repositories;
using HRM.Domain.Entities;
using HRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRM.Infrastructure.Persistence.Repositories;

public class PositionRepository : IPositionRepository
{
    private readonly ApplicationDbContext _context;

    public PositionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Position?> GetByIdAsync(Guid id)
    {
        return await _context.Positions
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Position?> GetByCodeAsync(string code)
    {
        return await _context.Positions
            .FirstOrDefaultAsync(x => x.Code == code);
    }

    public async Task<IReadOnlyList<Position>> GetAllAsync()
    {
        return await _context.Positions
            .OrderBy(x => x.Code)
            .ToListAsync();
    }

    public async Task AddAsync(Position position)
    {
        await _context.Positions.AddAsync(position);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Position position)
    {
        _context.Positions.Update(position);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Position position)
    {
        _context.Positions.Remove(position);
        await _context.SaveChangesAsync();
    }
}