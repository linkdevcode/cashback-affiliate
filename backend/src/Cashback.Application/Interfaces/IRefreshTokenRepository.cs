using Cashback.Domain.Entities;

namespace Cashback.Application.Interfaces;

/// <summary>
/// Persistence abstraction for user refresh tokens.
/// </summary>
public interface IRefreshTokenRepository
{
    /// <summary>
    /// Gets an active refresh token by its hash.
    /// </summary>
    Task<UserRefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken);

    /// <summary>
    /// Persists a new refresh token.
    /// </summary>
    Task AddAsync(UserRefreshToken refreshToken, CancellationToken cancellationToken);

    /// <summary>
    /// Persists changes to an existing refresh token.
    /// </summary>
    Task UpdateAsync(UserRefreshToken refreshToken, CancellationToken cancellationToken);

    /// <summary>
    /// Revokes all active refresh tokens for a user.
    /// </summary>
    Task RevokeAllForUserAsync(Guid userId, CancellationToken cancellationToken);
}
