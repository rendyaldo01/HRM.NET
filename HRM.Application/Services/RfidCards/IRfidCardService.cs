using HRM.Application.DTOs.RfidCards;

namespace HRM.Application.Services.RfidCards;

public interface IRfidCardService
{
    Task<RfidCardDto?> GetByIdAsync(Guid id);

    Task<RfidCardDto?> GetByCardUidAsync(string cardUid);

    Task<IReadOnlyList<RfidCardDto>> GetAllAsync();

    Task<IReadOnlyList<RfidCardDto>> GetAllByEmployeeIdAsync(
        Guid employeeId);

    Task<RfidCardDto> RegisterAsync(
        RegisterRfidCardRequest request);

    Task DeactivateAsync(Guid id);
}
