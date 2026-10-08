using HRM.Domain.Entities;

namespace HRM.Application.Interfaces.Repositories;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(Guid id);

    Task<Employee?> GetByEmployeeNumberAsync(string employeeNumber);

    Task<IReadOnlyList<Employee>> GetAllAsync();

    Task AddAsync(Employee employee);

    Task UpdateAsync(Employee employee);

    Task DeleteAsync(Employee employee);
}