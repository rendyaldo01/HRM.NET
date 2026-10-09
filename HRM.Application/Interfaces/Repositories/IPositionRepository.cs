using HRM.Domain.Entities;

namespace HRM.Application.Interfaces.Repositories;

public interface IPositionRepository
{
    Task<Position?> GetByIdAsync(Guid id);
    Task<Position?> GetByCodeAsync(string code);
    Task<IReadOnlyList<Position>> GetAllAsync();
    Task AddAsync(Position position);
    Task UpdateAsync(Position position);
    Task DeleteAsync(Position position);
}