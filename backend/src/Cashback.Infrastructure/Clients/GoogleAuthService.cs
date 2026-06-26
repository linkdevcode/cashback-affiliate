using Cashback.Application.Interfaces;
using Cashback.Infrastructure.Settings;
using Google.Apis.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cashback.Infrastructure.Clients;

/// <summary>
/// Google OAuth ID token validation client.
/// </summary>
public sealed class GoogleAuthService : IGoogleAuthService
{
    private readonly GoogleOptions _options;
    private readonly ILogger<GoogleAuthService> _logger;

    /// <summary>
    /// Initializes a new instance of the Google auth service.
    /// </summary>
    public GoogleAuthService(IOptions<GoogleOptions> options, ILogger<GoogleAuthService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<GoogleUserInfo?> ValidateIdTokenAsync(
        string idToken,
        CancellationToken cancellationToken)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [_options.ClientId]
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            if (string.IsNullOrWhiteSpace(payload.Email) || string.IsNullOrWhiteSpace(payload.Subject))
            {
                return null;
            }

            return new GoogleUserInfo
            {
                ProviderUserId = payload.Subject,
                Email = payload.Email,
                FullName = payload.Name ?? payload.Email,
                AvatarUrl = payload.Picture
            };
        }
        catch (InvalidJwtException ex)
        {
            _logger.LogWarning(ex, "Google ID token validation failed");
            return null;
        }
    }
}
