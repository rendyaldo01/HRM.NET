using HRM.Application.DTOs.Employees;
using HRM.Application.Interfaces.Repositories;
using HRM.Domain.Entities;
using HRM.Application.DTOs.Departments;
using HRM.Application.DTOs.Positions;

namespace HRM.Application.Services.Employees;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IPositionRepository _positionRepository;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository,
        IPositionRepository positionRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _positionRepository = positionRepository;
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

        employee.AssignDepartment(request.DepartmentId);
        employee.AssignPosition(request.PositionId);

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

        employee.AssignDepartment(request.DepartmentId);
        employee.AssignPosition(request.PositionId);

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

    public async Task ActivateAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            throw new KeyNotFoundException("Employee not found.");
        }

        employee.Activate();

        await _employeeRepository.UpdateAsync(employee);
    }

    public async Task DeactivateAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            throw new KeyNotFoundException("Employee not found.");
        }

        employee.Deactivate();

        await _employeeRepository.UpdateAsync(employee);
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
            DepartmentId = employee.DepartmentId,
            DepartmentName = employee.Department?.Name,
            PositionId = employee.PositionId,
            PositionName = employee.Position?.Name,
            IsActive = employee.IsActive
        };
    }

    public async Task<IReadOnlyList<DepartmentDto>> GetDepartmentsAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();

        return departments
            .Where(department => department.IsActive)
            .Select(department => new DepartmentDto
            {
                Id = department.Id,
                Code = department.Code,
                Name = department.Name,
                IsActive = department.IsActive
            })
            .ToList();
    }

    public async Task<IReadOnlyList<PositionDto>> GetPositionsAsync()
    {
        var positions = await _positionRepository.GetAllAsync();

        return positions
            .Where(position => position.IsActive)
            .Select(position => new PositionDto
            {
                Id = position.Id,
                Code = position.Code,
                Name = position.Name,
                IsActive = position.IsActive
            })
            .ToList();
    }

}