
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using HRM.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace HRM.Web.Security;

public class AttendanceDeviceAuthenticationHandler
    : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IAttendanceDeviceRepository _deviceRepository;

    public AttendanceDeviceAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IAttendanceDeviceRepository deviceRepository)
        : base(options, logger, encoder)
    {
        _deviceRepository = deviceRepository;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("X-API-Key", out var apiKeyValues))
        {
            return AuthenticateResult.NoResult();
        }

        var apiKey = apiKeyValues.ToString();

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return AuthenticateResult.Fail("API key is missing.");
        }

        var suppliedHash = Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(apiKey)));

        // DeviceCode is not supplied by the client for authentication.
        // We need to compare the hash against stored device hashes.
        var matchedDevice =
            await _deviceRepository.GetByApiKeyHashAsync(suppliedHash);

        if (matchedDevice is null || !matchedDevice.IsActive)
        {
            return AuthenticateResult.Fail(
                "Invalid API key or inactive device.");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, matchedDevice.Id.ToString()),
            new Claim(ClaimTypes.Name, matchedDevice.DeviceCode),
            new Claim("DeviceId", matchedDevice.Id.ToString()),
            new Claim("DeviceCode", matchedDevice.DeviceCode)
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
