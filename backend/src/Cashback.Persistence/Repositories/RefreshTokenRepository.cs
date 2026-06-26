using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Cashback.Persistence.Repositories;

/// <summary>
/// Entity Framework implementation of refresh token persistence.
/// </summary>
public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the refresh token repository.
    /// </summary>
    public RefreshTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<UserRefreshToken?> GetByTokenHashAsync(
        string tokenHash,
        CancellationToken cancellationToken)
    {
        return await _context.UserRefreshTokens
            .Include(token => token.User)
            .FirstOrDefaultAsync(token => token.TokenHash == tokenHash, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AddAsync(UserRefreshToken refreshToken, CancellationToken cancellationToken)
    {
        await _context.UserRefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(UserRefreshToken refreshToken, CancellationToken cancellationToken)
    {
        _context.UserRefreshTokens.Update(refreshToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task RevokeAllForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var activeTokens = await _context.UserRefreshTokens
            .Where(token => token.UserId == userId && token.RevokedAt == null)
            .ToListAsync(cancellationToken);

        foreach (var token in activeTokens)
        {
            token.Revoke();
        }

        if (activeTokens.Count > 0)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
