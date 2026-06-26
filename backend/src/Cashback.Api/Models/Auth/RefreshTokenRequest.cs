namespace Cashback.Api.Models.Auth;

/// <summary>
/// Refresh token request body.
/// </summary>
public sealed class RefreshTokenRequest
{
    /// <summary>
    /// Opaque refresh token issued at login.
    /// </summary>
    public string RefreshToken { get; init; } = string.Empty;
}
