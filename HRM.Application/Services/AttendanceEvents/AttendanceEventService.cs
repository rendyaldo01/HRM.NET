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
}