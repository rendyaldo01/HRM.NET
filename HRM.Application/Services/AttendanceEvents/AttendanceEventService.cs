using HRM.Application.DTOs.AttendanceEvents;
using HRM.Application.Interfaces.Repositories;
using HRM.Domain.Entities;

namespace HRM.Application.Services.AttendanceEvents;

public sealed class AttendanceEventService: IAttendanceEventService
{
    private readonly IAttendanceEventRepository _attendanceEventRepository;
    private readonly IRfidCardRepository _rfidCardRepository;

    public AttendanceEventService(
        IAttendanceEventRepository attendanceEventRepository,
        IRfidCardRepository rfidCardRepository)
    {
        _attendanceEventRepository = attendanceEventRepository;
        _rfidCardRepository = rfidCardRepository;
    }

    public async Task<bool> RecordAsync(Guid deviceId, RecordAttendanceEventRequest request)
    {
        var eventId = request.EventId.Trim();

        var alreadyExists =
            await _attendanceEventRepository.ExistsAsync(
                deviceId,
                eventId);

        if (alreadyExists)
        {
            return false;
        }

        var card = await _rfidCardRepository
            .GetByCardUidAsync(request.CardUid);

        if (card is null || !card.IsActive)
        {
            throw new InvalidOperationException(
                "RFID card is not registered or inactive.");
        }

        var occurredAtUtc = request.OccurredAt.UtcDateTime;

        var attendanceEvent = new AttendanceEvent(
            deviceId,
            eventId,
            request.CardUid,
            occurredAtUtc);

        var saved = await _attendanceEventRepository.AddAsync(
            attendanceEvent);

        return saved;
    }

    public async Task<IReadOnlyList<AttendanceEventHistoryDto>> GetHistoryByEmployeeAsync(
        Guid employeeId,
        DateTime fromUtc,
        DateTime toUtc)
    {
        if (employeeId == Guid.Empty)
        {
            throw new ArgumentException(
            "Employee ID is required.",
            nameof(employeeId));
        }

        if (fromUtc.Kind != DateTimeKind.Utc ||
            toUtc.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException(
                "Date range must be in UTC.");
        }

        if (fromUtc >= toUtc)
        {
            throw new ArgumentException(
                "The start date must be earlier than the end date.");
        }

        return await _attendanceEventRepository
            .GetHistoryByEmployeeAsync(
                employeeId,
                fromUtc,
                toUtc);

    }


    public async Task<IReadOnlyList<AttendanceEventHistoryDto>> GetAllHistoryAsync(
        DateTime fromUtc,
        DateTime toUtc)
    {
        if (fromUtc.Kind != DateTimeKind.Utc ||
            toUtc.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException(
                "Date range must be in UTC.");
        }

        if (fromUtc >= toUtc)
        {
            throw new ArgumentException(
                "The start date must be earlier than the end date.");
        }

        return await _attendanceEventRepository.GetAllHistoryAsync(
            fromUtc,
            toUtc);
    }

}