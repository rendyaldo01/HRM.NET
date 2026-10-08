using HRM.Application.DTOs.Employees;

namespace HRM.Application.Services.Employees;

public interface IEmployeeService
{
    Task<EmployeeDto?> GetByIdAsync(Guid id);

    Task<IReadOnlyList<EmployeeDto>> GetAllAsync();

    Task<EmployeeDto> CreateAsync(CreateEmployeeRequest request);

    Task UpdateAsync(Guid id, CreateEmployeeRequest request);

    Task DeleteAsync(Guid id);
}