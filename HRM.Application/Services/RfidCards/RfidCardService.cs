
using HRM.Application.DTOs.RfidCards;
using HRM.Application.Interfaces.Repositories;
using HRM.Application.Services.Employees;
using HRM.Domain.Entities;

namespace HRM.Application.Services.RfidCards;

public class RfidCardService : IRfidCardService
{
    private readonly IRfidCardRepository _rfidCardRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public RfidCardService(
        IRfidCardRepository rfidCardRepository,
        IEmployeeRepository employeeRepository)
    {
        _rfidCardRepository = rfidCardRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<RfidCardDto?> GetByIdAsync(Guid id)
    {
        var card = await _rfidCardRepository.GetByIdAsync(id);

        return card == null ? null : MapToDto(card);
    }

    public async Task<RfidCardDto?> GetByCardUidAsync(string cardUid)
    {
        if (string.IsNullOrWhiteSpace(cardUid))
        {
            throw new ArgumentException("Card UID is required.");
        }

        var card = await _rfidCardRepository
            .GetByCardUidAsync(cardUid);

        return card == null ? null : MapToDto(card);
    }

    public async Task<IReadOnlyList<RfidCardDto>> GetAllByEmployeeIdAsync(
        Guid employeeId)
    {
        var cards = await _rfidCardRepository
            .GetAllByEmployeeIdAsync(employeeId);

        return cards.Select(MapToDto).ToList();
    }

    public async Task<RfidCardDto> RegisterAsync(
        RegisterRfidCardRequest request)
    {
        if (request.EmployeeId == Guid.Empty)
        {
            throw new ArgumentException("Employee ID is required.");
        }

        if (string.IsNullOrWhiteSpace(request.CardUid))
        {
            throw new ArgumentException("Card UID is required.");
        }

        var employee = await _employeeRepository
            .GetByIdAsync(request.EmployeeId);

        if (employee == null)
        {
            throw new KeyNotFoundException("Employee not found.");
        }

        if (!employee.IsActive)
        {
            throw new InvalidOperationException(
                "Cannot register a card for an inactive employee.");
        }

        var normalizedUid = request.CardUid.Trim().ToUpperInvariant();

        var existingCard = await _rfidCardRepository
            .GetByCardUidAsync(normalizedUid);

        if (existingCard != null)
        {
            throw new InvalidOperationException(
                "This RFID card UID has already been registered.");
        }

        var activeCard = await _rfidCardRepository
            .GetActiveByEmployeeIdAsync(request.EmployeeId);

        if (activeCard != null)
        {
            throw new InvalidOperationException(
                "Employee already has an active RFID card. " +
                "Deactivate the existing card first.");
        }

        var card = new RfidCard(
            request.EmployeeId,
            normalizedUid);

        await _rfidCardRepository.AddAsync(card);

        return new RfidCardDto
        {
            Id = card.Id,
            EmployeeId = card.EmployeeId,
            EmployeeNumber = employee.EmployeeNumber,
            EmployeeName = $"{employee.FirstName} {employee.LastName}".Trim(),
            CardUid = card.CardUid,
            IsActive = card.IsActive,
            CreatedAtUtc = card.CreatedAtUtc
        };
    }

    public async Task DeactivateAsync(Guid id)
    {
        var card = await _rfidCardRepository.GetByIdAsync(id);

        if (card == null)
        {
            throw new KeyNotFoundException("RFID card not found.");
        }

        if (!card.IsActive)
        {
            throw new InvalidOperationException(
                "RFID card is already inactive.");
        }

        card.Deactivate();

        await _rfidCardRepository.UpdateAsync(card);
    }

    private static RfidCardDto MapToDto(RfidCard card)
    {
        return new RfidCardDto
        {
            Id = card.Id,
            EmployeeId = card.EmployeeId,
            EmployeeNumber = card.Employee.EmployeeNumber,
            EmployeeName =
                $"{card.Employee.FirstName} {card.Employee.LastName}".Trim(),
            CardUid = card.CardUid,
            IsActive = card.IsActive,
            CreatedAtUtc = card.CreatedAtUtc
        };
    }
}
