namespace Cashback.Application.Features.Auth.RefreshToken;

/// <summary>
/// Response returned after a successful token refresh.
/// </summary>
public sealed class RefreshTokenResponse
{
    /// <summary>
    /// New JWT access token.
    /// </summary>
    public string AccessToken { get; init; } = null!;
}
