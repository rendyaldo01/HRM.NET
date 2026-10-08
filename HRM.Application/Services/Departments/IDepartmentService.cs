using HRM.Application.DTOs.Departments;

namespace HRM.Application.Services.Departments;

public interface IDepartmentService
{
    Task<DepartmentDto?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto> CreateAsync(DepartmentRequest request);
    Task UpdateAsync(Guid id, DepartmentRequest request);
    Task DeleteAsync(Guid id);
    Task ActivateAsync(Guid id);
    Task DeactivateAsync(Guid id);
}