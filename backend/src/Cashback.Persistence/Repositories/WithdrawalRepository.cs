using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Cashback.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Cashback.Persistence.Repositories;

/// <summary>
/// Entity Framework implementation of withdrawal persistence.
/// </summary>
public class WithdrawalRepository : IWithdrawalRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the withdrawal repository.
    /// </summary>
    public WithdrawalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task AddAsync(Withdrawal withdrawal, CancellationToken cancellationToken)
    {
        await _context.Withdrawals.AddAsync(withdrawal, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Withdrawal?> GetByIdForUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await _context.Withdrawals
            .AsNoTracking()
            .FirstOrDefaultAsync(
                withdrawal => withdrawal.Id == id && withdrawal.UserId == userId,
                cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Withdrawal?> GetByIdForUpdateAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Withdrawals
            .FirstOrDefaultAsync(withdrawal => withdrawal.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<(IReadOnlyList<Withdrawal> Items, int TotalCount)> GetPagedByUserIdAsync(
        Guid userId,
        int page,
        int pageSize,
        WithdrawalStatus? status,
        string sortBy,
        string sortDirection,
        CancellationToken cancellationToken)
    {
        var query = _context.Withdrawals
            .AsNoTracking()
            .Where(withdrawal => withdrawal.UserId == userId);

        if (status.HasValue)
        {
            query = query.Where(withdrawal => withdrawal.Status == status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await ApplySorting(query, sortBy, sortDirection)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    /// <summary>
    /// Applies sorting to a withdrawal query.
    /// </summary>
    private static IQueryable<Withdrawal> ApplySorting(
        IQueryable<Withdrawal> query,
        string sortBy,
        string sortDirection)
    {
        var descending = sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

        return sortBy.ToLowerInvariant() switch
        {
            "amount" => descending
                ? query.OrderByDescending(withdrawal => withdrawal.Amount)
                : query.OrderBy(withdrawal => withdrawal.Amount),
            "status" => descending
                ? query.OrderByDescending(withdrawal => withdrawal.Status)
                : query.OrderBy(withdrawal => withdrawal.Status),
            "processedat" => descending
                ? query.OrderByDescending(withdrawal => withdrawal.ProcessedAt)
                : query.OrderBy(withdrawal => withdrawal.ProcessedAt),
            _ => descending
                ? query.OrderByDescending(withdrawal => withdrawal.RequestedAt)
                : query.OrderBy(withdrawal => withdrawal.RequestedAt)
        };
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Withdrawal withdrawal, CancellationToken cancellationToken)
    {
        _context.Withdrawals.Update(withdrawal);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<decimal> GetTotalCompletedAmountByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await _context.Withdrawals
            .AsNoTracking()
            .Where(withdrawal => withdrawal.UserId == userId && withdrawal.Status == WithdrawalStatus.Completed)
            .SumAsync(withdrawal => withdrawal.Amount, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<decimal> GetTotalAmountByUserIdAndStatusesAsync(
        Guid userId,
        IReadOnlyList<WithdrawalStatus> statuses,
        CancellationToken cancellationToken)
    {
        return await _context.Withdrawals
            .AsNoTracking()
            .Where(withdrawal => withdrawal.UserId == userId && statuses.Contains(withdrawal.Status))
            .SumAsync(withdrawal => withdrawal.Amount, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task CreateRequestWithTransactionAsync(
        Withdrawal withdrawal,
        Transaction transaction,
        CancellationToken cancellationToken)
    {
        await _context.Withdrawals.AddAsync(withdrawal, cancellationToken);
        await _context.Transactions.AddAsync(transaction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task ProcessWithdrawalUpdateAsync(
        Withdrawal withdrawal,
        Transaction transaction,
        CancellationToken cancellationToken)
    {
        _context.Withdrawals.Update(withdrawal);
        await _context.Transactions.AddAsync(transaction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<AdminWithdrawalStatistics> GetAdminStatisticsAsync(CancellationToken cancellationToken)
    {
        var withdrawals = _context.Withdrawals.AsNoTracking();

        return new AdminWithdrawalStatistics(
            await withdrawals.CountAsync(cancellationToken),
            await withdrawals.CountAsync(
                withdrawal => withdrawal.Status == WithdrawalStatus.Pending,
                cancellationToken),
            await withdrawals.CountAsync(
                withdrawal => withdrawal.Status == WithdrawalStatus.Completed,
                cancellationToken));
    }

    /// <inheritdoc/>
    public async Task<WithdrawalUserSummary> GetUserSummaryAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var withdrawals = _context.Withdrawals
            .AsNoTracking()
            .Where(withdrawal => withdrawal.UserId == userId);

        return new WithdrawalUserSummary(
            await withdrawals.CountAsync(cancellationToken),
            await withdrawals.CountAsync(
                withdrawal => withdrawal.Status == WithdrawalStatus.Pending,
                cancellationToken),
            await withdrawals.CountAsync(
                withdrawal => withdrawal.Status == WithdrawalStatus.Approved,
                cancellationToken),
            await withdrawals.CountAsync(
                withdrawal => withdrawal.Status == WithdrawalStatus.Rejected,
                cancellationToken),
            await withdrawals.CountAsync(
                withdrawal => withdrawal.Status == WithdrawalStatus.Completed,
                cancellationToken),
            await withdrawals
                .Where(withdrawal => withdrawal.Status == WithdrawalStatus.Completed)
                .SumAsync(withdrawal => withdrawal.Amount, cancellationToken));
    }

    /// <inheritdoc/>
    public async Task<(IReadOnlyList<Withdrawal> Items, int TotalCount)> GetPagedForAdminAsync(
        int page,
        int pageSize,
        string? user,
        WithdrawalStatus? status,
        CancellationToken cancellationToken)
    {
        var query = _context.Withdrawals
            .AsNoTracking()
            .Include(withdrawal => withdrawal.User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(user))
        {
            var userTerm = $"%{user.Trim()}%";
            query = query.Where(withdrawal =>
                EF.Functions.ILike(withdrawal.User.Email, userTerm) ||
                EF.Functions.ILike(withdrawal.User.FullName, userTerm));
        }

        if (status.HasValue)
        {
            query = query.Where(withdrawal => withdrawal.Status == status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(withdrawal => withdrawal.RequestedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    /// <inheritdoc/>
    public async Task<Withdrawal?> GetByIdForAdminAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Withdrawals
            .AsNoTracking()
            .Include(withdrawal => withdrawal.User)
            .FirstOrDefaultAsync(withdrawal => withdrawal.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Withdrawal>> GetRecentForAdminAsync(
        int count,
        CancellationToken cancellationToken)
    {
        return await _context.Withdrawals
            .AsNoTracking()
            .Include(withdrawal => withdrawal.User)
            .OrderByDescending(withdrawal => withdrawal.RequestedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }
}
