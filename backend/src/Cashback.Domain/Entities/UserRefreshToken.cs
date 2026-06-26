using Cashback.Domain.Common;

namespace Cashback.Domain.Entities;

/// <summary>
/// Stored refresh token for JWT session renewal.
/// </summary>
public class UserRefreshToken : BaseEntity
{
    /// <summary>
    /// Owner of the refresh token.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Hashed refresh token value.
    /// </summary>
    public string TokenHash { get; private set; } = null!;

    /// <summary>
    /// UTC timestamp when the token expires.
    /// </summary>
    public DateTime ExpiresAt { get; private set; }

    /// <summary>
    /// UTC timestamp when the token was revoked.
    /// </summary>
    public DateTime? RevokedAt { get; private set; }

    /// <summary>
    /// UTC timestamp when the token was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// User associated with the refresh token.
    /// </summary>
    public User User { get; private set; } = null!;

    /// <summary>
    /// Required by Entity Framework.
    /// </summary>
    private UserRefreshToken()
    {
    }

    /// <summary>
    /// Creates a new refresh token record.
    /// </summary>
    public static UserRefreshToken Create(Guid userId, string tokenHash, DateTime expiresAt)
    {
        return new UserRefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = tokenHash,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Indicates whether the token can still be used.
    /// </summary>
    public bool IsActive()
    {
        return RevokedAt is null && ExpiresAt > DateTime.UtcNow;
    }

    /// <summary>
    /// Revokes the refresh token.
    /// </summary>
    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
    }
}
