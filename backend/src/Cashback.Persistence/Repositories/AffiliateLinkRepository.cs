using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Cashback.Persistence.Repositories;

/// <summary>
/// Entity Framework implementation of affiliate link persistence.
/// </summary>
public class AffiliateLinkRepository : IAffiliateLinkRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the affiliate link repository.
    /// </summary>
    public AffiliateLinkRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<AffiliateLink?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.AffiliateLinks
            .AsNoTracking()
            .FirstOrDefaultAsync(link => link.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<AffiliateLink?> GetByIdForUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await _context.AffiliateLinks
            .AsNoTracking()
            .FirstOrDefaultAsync(link => link.Id == id && link.UserId == userId, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<(IReadOnlyList<AffiliateLink> Items, int TotalCount)> GetPagedByUserIdAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = _context.AffiliateLinks
            .AsNoTracking()
            .Where(link => link.UserId == userId);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(link => link.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    /// <inheritdoc/>
    public async Task AddAsync(AffiliateLink affiliateLink, CancellationToken cancellationToken)
    {
        await _context.AffiliateLinks.AddAsync(affiliateLink, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
