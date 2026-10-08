using HRM.Application.Interfaces.Repositories;
using HRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRM.Infrastructure.Persistence.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;

    public DepartmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Department?> GetByIdAsync(Guid id)
    {
        return await _context.Departments
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Department?> GetByCodeAsync(string code)
    {
        return await _context.Departments
            .FirstOrDefaultAsync(x => x.Code == code);
    }

    public async Task<IReadOnlyList<Department>> GetAllAsync()
    {
        return await _context.Departments
            .AsNoTracking()
            .OrderBy(x => x.Code)
            .ToListAsync();
    }

    public async Task AddAsync(Department department)
    {
        await _context.Departments.AddAsync(department);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Department department)
    {
        _context.Departments.Update(department);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Department department)
    {
        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
    }
}