using Cashback.Domain.Entities;
using Cashback.Domain.Enums;

namespace Cashback.Application.Interfaces;

/// <summary>
/// Persistence abstraction for withdrawal requests.
/// </summary>
public interface IWithdrawalRepository
{
    /// <summary>
    /// Persists a new withdrawal request.
    /// </summary>
    Task AddAsync(Withdrawal withdrawal, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a withdrawal by identifier for a specific user.
    /// </summary>
    Task<Withdrawal?> GetByIdForUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets a withdrawal by identifier for update operations.
    /// </summary>
    Task<Withdrawal?> GetByIdForUpdateAsync(
        Guid id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets paginated withdrawals for a user.
    /// </summary>
    Task<(IReadOnlyList<Withdrawal> Items, int TotalCount)> GetPagedByUserIdAsync(
        Guid userId,
        int page,
        int pageSize,
        WithdrawalStatus? status,
        string sortBy,
        string sortDirection,
        CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing withdrawal request.
    /// </summary>
    Task UpdateAsync(Withdrawal withdrawal, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the total amount of completed withdrawals for a user.
    /// </summary>
    Task<decimal> GetTotalCompletedAmountByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets the total withdrawal amount for a user across the given statuses.
    /// </summary>
    Task<decimal> GetTotalAmountByUserIdAndStatusesAsync(
        Guid userId,
        IReadOnlyList<WithdrawalStatus> statuses,
        CancellationToken cancellationToken);

    /// <summary>
    /// Persists a withdrawal request and its financial transaction atomically.
    /// </summary>
    Task CreateRequestWithTransactionAsync(
        Withdrawal withdrawal,
        Transaction transaction,
        CancellationToken cancellationToken);

    /// <summary>
    /// Persists withdrawal status changes and a financial transaction atomically.
    /// </summary>
    Task ProcessWithdrawalUpdateAsync(
        Withdrawal withdrawal,
        Transaction transaction,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets aggregated withdrawal statistics for the admin dashboard.
    /// </summary>
    Task<AdminWithdrawalStatistics> GetAdminStatisticsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets aggregated withdrawal statistics for a user.
    /// </summary>
    Task<WithdrawalUserSummary> GetUserSummaryAsync(
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets paginated withdrawals for admin management.
    /// </summary>
    Task<(IReadOnlyList<Withdrawal> Items, int TotalCount)> GetPagedForAdminAsync(
        int page,
        int pageSize,
        string? user,
        WithdrawalStatus? status,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets a withdrawal by identifier for admin detail views.
    /// </summary>
    Task<Withdrawal?> GetByIdForAdminAsync(
        Guid id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets the most recent withdrawals across the platform for admin activity widgets.
    /// </summary>
    Task<IReadOnlyList<Withdrawal>> GetRecentForAdminAsync(
        int count,
        CancellationToken cancellationToken);
}
