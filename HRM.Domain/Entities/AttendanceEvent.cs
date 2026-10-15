
namespace HRM.Domain.Entities;

public class AttendanceEvent
{
    public Guid Id { get; private set; }

    public Guid DeviceId { get; private set; }

    public string EventId { get; private set; } = string.Empty;

    public string CardUid { get; private set; } = string.Empty;

    public DateTime OccurredAtUtc { get; private set; }

    public DateTime ReceivedAtUtc { get; private set; }

    private AttendanceEvent()
    {
    }

    public AttendanceEvent(
        Guid deviceId,
        string eventId,
        string cardUid,
        DateTime occurredAtUtc)
    {
        if (deviceId == Guid.Empty)
            throw new ArgumentException("Device ID is required.");

        if (string.IsNullOrWhiteSpace(eventId))
            throw new ArgumentException("Event ID is required.");

        if (string.IsNullOrWhiteSpace(cardUid))
            throw new ArgumentException("Card UID is required.");

        if (occurredAtUtc.Kind != DateTimeKind.Utc)
            throw new ArgumentException(
                "Occurred time must be converted to UTC.");

        Id = Guid.NewGuid();
        DeviceId = deviceId;
        EventId = eventId.Trim();
        CardUid = cardUid.Trim().ToUpperInvariant();
        OccurredAtUtc = occurredAtUtc;
        ReceivedAtUtc = DateTime.UtcNow;
    }
}
