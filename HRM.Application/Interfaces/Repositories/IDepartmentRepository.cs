using HRM.Domain.Entities;

namespace HRM.Application.Interfaces.Repositories;

public interface IDepartmentRepository
{
    Task<Department?> GetByIdAsync(Guid id);
    Task<Department?> GetByCodeAsync(string code);
    Task<IReadOnlyList<Department>> GetAllAsync();
    Task AddAsync(Department department);
    Task UpdateAsync(Department department);
    Task DeleteAsync(Department department);
}