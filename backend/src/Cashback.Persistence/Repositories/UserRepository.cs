using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Cashback.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Cashback.Persistence.Repositories;

/// <summary>
/// Entity Framework implementation of user persistence.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the user repository.
    /// </summary>
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<User?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<User?> GetByProviderUserIdAsync(
        string providerUserId,
        CancellationToken cancellationToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                user => user.ProviderUserId == providerUserId && user.Provider == AuthProvider.Google,
                cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(user => user.Email == email, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<AdminUserStatistics> GetAdminStatisticsAsync(CancellationToken cancellationToken)
    {
        var users = _context.Users.AsNoTracking();

        return new AdminUserStatistics(
            await users.CountAsync(user => user.Status != UserStatus.Deleted, cancellationToken),
            await users.CountAsync(user => user.Status == UserStatus.Active, cancellationToken),
            await users.CountAsync(user => user.Status == UserStatus.Suspended, cancellationToken));
    }

    /// <inheritdoc/>
    public async Task<(IReadOnlyList<User> Items, int TotalCount)> GetPagedForAdminAsync(
        int page,
        int pageSize,
        string? email,
        string? name,
        UserStatus? status,
        CancellationToken cancellationToken)
    {
        var query = _context.Users
            .AsNoTracking()
            .Where(user => user.Status != UserStatus.Deleted);

        if (status.HasValue)
        {
            query = query.Where(user => user.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            var emailTerm = $"%{email.Trim()}%";
            query = query.Where(user => EF.Functions.ILike(user.Email, emailTerm));
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            var nameTerm = $"%{name.Trim()}%";
            query = query.Where(user => EF.Functions.ILike(user.FullName, nameTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(user => user.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<User>> GetRecentForAdminAsync(
        int count,
        CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(user => user.Status != UserStatus.Deleted)
            .OrderByDescending(user => user.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }
}
