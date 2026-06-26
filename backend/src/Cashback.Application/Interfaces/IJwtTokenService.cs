using Cashback.Domain.Entities;

namespace Cashback.Application.Interfaces;

/// <summary>
/// JWT access and refresh token generation service.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generates a signed JWT access token for the user.
    /// </summary>
    string GenerateAccessToken(User user);

    /// <summary>
    /// Generates a cryptographically secure refresh token.
    /// </summary>
    string GenerateRefreshToken();

    /// <summary>
    /// Hashes a refresh token for secure storage.
    /// </summary>
    string HashRefreshToken(string refreshToken);

    /// <summary>
    /// Calculates the UTC expiry time for a new refresh token.
    /// </summary>
    DateTime GetRefreshTokenExpiry();

    /// <summary>
    /// Reads user identifier from an expired or valid access token.
    /// </summary>
    Guid? GetUserIdFromExpiredToken(string accessToken);
}
