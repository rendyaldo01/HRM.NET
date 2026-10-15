
namespace HRM.Domain.Entities;

public class AttendanceDevice
{
    public Guid Id { get; private set; }

    public string DeviceCode { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string ApiKeyHash { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    private AttendanceDevice()
    {
    }

    public AttendanceDevice(
        string deviceCode,
        string name,
        string apiKeyHash)
    {
        if (string.IsNullOrWhiteSpace(deviceCode))
            throw new ArgumentException("Device code is required.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Device name is required.");

        if (string.IsNullOrWhiteSpace(apiKeyHash))
            throw new ArgumentException("API key hash is required.");

        Id = Guid.NewGuid();
        DeviceCode = deviceCode.Trim().ToUpperInvariant();
        Name = name.Trim();
        ApiKeyHash = apiKeyHash;
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void UpdateDetails(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Device name is required.");

        Name = name.Trim();
    }
    public void RotateApiKey(string newApiKeyHash)
    {
        if (string.IsNullOrWhiteSpace(newApiKeyHash))
            throw new ArgumentException("API key hash is required.");

        ApiKeyHash = newApiKeyHash;
    }
}
