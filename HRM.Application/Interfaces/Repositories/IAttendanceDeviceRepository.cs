using HRM.Domain.Entities;

namespace HRM.Application.Interfaces.Repositories;

public interface IAttendanceDeviceRepository
{
    Task<AttendanceDevice?> GetByIdAsync(Guid id);

    Task<AttendanceDevice?> GetByDeviceCodeAsync(
        string deviceCode);

    Task<IReadOnlyList<AttendanceDevice>> GetAllAsync();

    Task AddAsync(AttendanceDevice device);

    Task UpdateAsync(AttendanceDevice device);

    Task<AttendanceDevice?> GetByApiKeyHashAsync(string apiKeyHash);

}
