using HRM.Application.DTOs.Positions;
using HRM.Application.Interfaces.Repositories;
using HRM.Domain.Entities;

namespace HRM.Application.Services.Positions;

public class PositionService : IPositionService
{
    private readonly IPositionRepository _positionRepository;

    public PositionService(IPositionRepository positionRepository)
    {
        _positionRepository = positionRepository;
    }

    public async Task<IReadOnlyList<PositionDto>> GetAllAsync()
    {
        var positions = await _positionRepository.GetAllAsync();

        return positions.Select(position => new PositionDto
        {
            Id = position.Id,
            Code = position.Code,
            Name = position.Name,
            IsActive = position.IsActive
        }).ToList();
    }

    public async Task<PositionDto?> GetByIdAsync(Guid id)
    {
        var position = await _positionRepository.GetByIdAsync(id);

        if (position is null)
            return null;

        return new PositionDto
        {
            Id = position.Id,
            Code = position.Code,
            Name = position.Name,
            IsActive = position.IsActive
        };
    }

    public async Task CreateAsync(PositionRequest request)
    {
        var code = request.Code.Trim();
        var name = request.Name.Trim();

        var existing = await _positionRepository.GetByCodeAsync(code);

        if (existing is not null)
            throw new InvalidOperationException(
                $"Position code '{code}' already exists.");

        var position = new Position(code, name);

        await _positionRepository.AddAsync(position);
    }

    public async Task UpdateAsync(Guid id, PositionRequest request)
    {
        var position = await _positionRepository.GetByIdAsync(id);

        if (position is null)
            throw new KeyNotFoundException("Position not found.");

        var code = request.Code.Trim();
        var name = request.Name.Trim();

        var existing = await _positionRepository.GetByCodeAsync(code);

        if (existing is not null && existing.Id != id)
            throw new InvalidOperationException(
                $"Position code '{code}' already exists.");

        position.Update(code, name);

        await _positionRepository.UpdateAsync(position);
    }

    public async Task ActivateAsync(Guid id)
    {
        var position = await _positionRepository.GetByIdAsync(id);

        if (position is null)
            throw new KeyNotFoundException("Position not found.");

        position.Activate();

        await _positionRepository.UpdateAsync(position);
    }

    public async Task DeactivateAsync(Guid id)
    {
        var position = await _positionRepository.GetByIdAsync(id);

        if (position is null)
            throw new KeyNotFoundException("Position not found.");

        position.Deactivate();

        await _positionRepository.UpdateAsync(position);
    }
}