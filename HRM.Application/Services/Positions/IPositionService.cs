using HRM.Application.DTOs.Positions;

namespace HRM.Application.Services.Positions;

public interface IPositionService
{
    Task<IReadOnlyList<PositionDto>> GetAllAsync();
    Task<PositionDto?> GetByIdAsync(Guid id);
    Task CreateAsync(PositionRequest request);
    Task UpdateAsync(Guid id, PositionRequest request);
    Task ActivateAsync(Guid id);
    Task DeactivateAsync(Guid id);
}