
using HRM.Domain.Entities;

namespace HRM.Application.Interfaces.Repositories;

public interface IRfidCardRepository
{
    Task<RfidCard?> GetByIdAsync(Guid id);

    Task<RfidCard?> GetByCardUidAsync(string cardUid);

    Task<RfidCard?> GetActiveByEmployeeIdAsync(Guid employeeId);

    Task<IReadOnlyList<RfidCard>> GetAllByEmployeeIdAsync(
        Guid employeeId);

    Task<IReadOnlyList<RfidCard>> GetAllAsync();

    Task AddAsync(RfidCard rfidCard);

    Task UpdateAsync(RfidCard rfidCard);
}
