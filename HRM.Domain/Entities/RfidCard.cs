namespace HRM.Domain.Entities;

public class RfidCard
{
    public Guid Id { get; private set; }

    public Guid EmployeeId { get; private set; }

    public Employee Employee { get; private set; } = null!;

    public string CardUid { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    private RfidCard()
    {
        // Digunakan oleh Entity Framework Core.
    }

    public RfidCard(Guid employeeId, string cardUid)
    {
        if (employeeId == Guid.Empty)
        {
            throw new ArgumentException(
                "Employee ID is required.",
                nameof(employeeId));
        }

        if (string.IsNullOrWhiteSpace(cardUid))
        {
            throw new ArgumentException(
                "Card UID is required.",
                nameof(cardUid));
        }

        Id = Guid.NewGuid();
        EmployeeId = employeeId;
        CardUid = cardUid.Trim().ToUpperInvariant();
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
