using HRM.Application.DTOs.Departments;
using HRM.Application.Interfaces.Repositories;
using HRM.Domain.Entities;

namespace HRM.Application.Services.Departments;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<DepartmentDto?> GetByIdAsync(Guid id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);

        if (department == null)
            return null;

        return MapToDto(department);
    }

    public async Task<IReadOnlyList<DepartmentDto>> GetAllAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();

        return departments
            .Select(MapToDto)
            .ToList();
    }

    public async Task<DepartmentDto> CreateAsync(DepartmentRequest request)
    {
        var existingDepartment =
            await _departmentRepository.GetByCodeAsync(request.Code);

        if (existingDepartment != null)
        {
            throw new InvalidOperationException(
                "Department code already exists.");
        }

        var department = new Department(
            request.Code,
            request.Name);

        await _departmentRepository.AddAsync(department);

        return MapToDto(department);
    }

    public async Task UpdateAsync(
        Guid id,
        DepartmentRequest request)
    {
        var department =
            await _departmentRepository.GetByIdAsync(id);

        if (department == null)
        {
            throw new KeyNotFoundException(
                "Department not found.");
        }

        department.Update(
            request.Code,
            request.Name);

        await _departmentRepository.UpdateAsync(department);
    }

    public async Task DeleteAsync(Guid id)
    {
        var department =
            await _departmentRepository.GetByIdAsync(id);

        if (department == null)
        {
            throw new KeyNotFoundException(
                "Department not found.");
        }

        await _departmentRepository.DeleteAsync(department);
    }

    public async Task ActivateAsync(Guid id)
    {
        var department =
            await _departmentRepository.GetByIdAsync(id);

        if (department == null)
        {
            throw new KeyNotFoundException(
                "Department not found.");
        }

        department.Activate();

        await _departmentRepository.UpdateAsync(department);
    }

    public async Task DeactivateAsync(Guid id)
    {
        var department =
            await _departmentRepository.GetByIdAsync(id);

        if (department == null)
        {
            throw new KeyNotFoundException(
                "Department not found.");
        }

        department.Deactivate();

        await _departmentRepository.UpdateAsync(department);
    }

    private static DepartmentDto MapToDto(
        Department department)
    {
        return new DepartmentDto
        {
            Id = department.Id,
            Code = department.Code,
            Name = department.Name,
            IsActive = department.IsActive
        };
    }
}