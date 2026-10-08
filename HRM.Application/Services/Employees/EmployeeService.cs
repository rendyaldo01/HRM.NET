using HRM.Application.DTOs.Employees;
using HRM.Application.Interfaces.Repositories;
using HRM.Domain.Entities;

namespace HRM.Application.Services.Employees;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<EmployeeDto?> GetByIdAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
            return null;

        return MapToDto(employee);
    }

    public async Task<IReadOnlyList<EmployeeDto>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();

        return employees
            .Select(MapToDto)
            .ToList();
    }

    public async Task<EmployeeDto> CreateAsync(
        CreateEmployeeRequest request)
    {
        var existingEmployee =
            await _employeeRepository
                .GetByEmployeeNumberAsync(request.EmployeeNumber);

        if (existingEmployee != null)
        {
            throw new InvalidOperationException(
                "Employee number already exists.");
        }

        var employee = new Employee(
            request.EmployeeNumber,
            request.FirstName,
            request.LastName,
            request.Email,
            request.HireDate);

        await _employeeRepository.AddAsync(employee);

        return MapToDto(employee);
    }

    public async Task UpdateAsync(
        Guid id,
        CreateEmployeeRequest request)
    {
        var employee =
            await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            throw new KeyNotFoundException(
                "Employee not found.");
        }

        employee.Update(
            request.FirstName,
            request.LastName,
            request.Email,
            request.HireDate);

        await _employeeRepository.UpdateAsync(employee);
    }

    public async Task DeleteAsync(Guid id)
    {
        var employee =
            await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            throw new KeyNotFoundException(
                "Employee not found.");
        }

        await _employeeRepository.DeleteAsync(employee);
    }

    public async Task<int> GetCountAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();

        return employees.Count;
    }

    private static EmployeeDto MapToDto(Employee employee)
    {
        return new EmployeeDto
        {
            Id = employee.Id,
            EmployeeNumber = employee.EmployeeNumber,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            HireDate = employee.HireDate,
            IsActive = employee.IsActive
        };
    }
}