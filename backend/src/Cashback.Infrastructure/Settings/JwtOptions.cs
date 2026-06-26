namespace Cashback.Infrastructure.Settings;

/// <summary>
/// JWT authentication configuration options.
/// </summary>
public sealed class JwtOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string SectionName = "Jwt";

    /// <summary>
    /// Signing secret for JWT tokens.
    /// </summary>
    public string Secret { get; init; } = string.Empty;

    /// <summary>
    /// Token issuer claim value.
    /// </summary>
    public string Issuer { get; init; } = string.Empty;

    /// <summary>
    /// Token audience claim value.
    /// </summary>
    public string Audience { get; init; } = string.Empty;

    /// <summary>
    /// Access token lifetime in minutes.
    /// </summary>
    public int AccessTokenExpirationMinutes { get; init; } = 15;

    /// <summary>
    /// Refresh token lifetime in days.
    /// </summary>
    public int RefreshTokenExpirationDays { get; init; } = 7;
}
