using System.Security.Cryptography;
using HRM.Application.DTOs.AttendanceDevices;
using HRM.Application.Interfaces.Repositories;
using HRM.Domain.Entities;

namespace HRM.Application.Services.AttendanceDevices;

public class AttendanceDeviceService : IAttendanceDeviceService
{
    private readonly IAttendanceDeviceRepository _deviceRepository;

    public AttendanceDeviceService(
        IAttendanceDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<IReadOnlyList<AttendanceDeviceListDto>> GetAllAsync()
    {
        var devices = await _deviceRepository.GetAllAsync();

        return devices.Select(device => new AttendanceDeviceListDto
        {
            Id = device.Id,
            DeviceCode = device.DeviceCode,
            Name = device.Name,
            IsActive = device.IsActive,
            CreatedAtUtc = device.CreatedAtUtc
        }).ToList();
    }

    public async Task<AttendanceDeviceRegistrationDto> RegisterAsync(
        RegisterAttendanceDeviceRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.DeviceCode))
        {
            throw new ArgumentException(
                "Device code is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException(
                "Device name is required.");
        }

        var deviceCode = request.DeviceCode.Trim().ToUpperInvariant();
        var name = request.Name.Trim();

        if (deviceCode.Length > 50)
        {
            throw new ArgumentException(
                "Device code cannot exceed 50 characters.");
        }

        if (name.Length > 100)
        {
            throw new ArgumentException(
                "Device name cannot exceed 100 characters.");
        }

        var existingDevice =
            await _deviceRepository.GetByDeviceCodeAsync(deviceCode);

        if (existingDevice != null)
        {
            throw new InvalidOperationException(
                "Device code already exists.");
        }

        // Generate a cryptographically secure 256-bit API key.
        var apiKey = Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(32));

        // Store only the SHA-256 hash in the database.
        var apiKeyHash = Convert.ToHexString(
            SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(apiKey)));

        var device = new AttendanceDevice(
            deviceCode,
            name,
            apiKeyHash);

        await _deviceRepository.AddAsync(device);

        return new AttendanceDeviceRegistrationDto
        {
            Id = device.Id,
            DeviceCode = device.DeviceCode,
            Name = device.Name,
            ApiKey = apiKey,
            CreatedAtUtc = device.CreatedAtUtc
        };
    }

    public async Task<AttendanceDeviceRegistrationDto> RotateApiKeyAsync(
    Guid deviceId)
    {
        var device = await _deviceRepository.GetByIdAsync(deviceId);

        if (device is null)
        {
            throw new KeyNotFoundException(
                "Attendance device was not found.");
        }

        // Generate a new cryptographically secure 256-bit API key.
        var apiKey = Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(32));

        // Store only the SHA-256 hash.
        var apiKeyHash = Convert.ToHexString(
            SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(apiKey)));

        device.RotateApiKey(apiKeyHash);

        await _deviceRepository.UpdateAsync(device);

        return new AttendanceDeviceRegistrationDto
        {
            Id = device.Id,
            DeviceCode = device.DeviceCode,
            Name = device.Name,
            ApiKey = apiKey,
            CreatedAtUtc = device.CreatedAtUtc
        };
    }
}
