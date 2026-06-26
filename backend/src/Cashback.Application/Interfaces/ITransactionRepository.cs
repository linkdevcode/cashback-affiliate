using Cashback.Domain.Entities;
using Cashback.Domain.Enums;

namespace Cashback.Application.Interfaces;

/// <summary>
/// Persistence abstraction for financial transactions.
/// </summary>
public interface ITransactionRepository
{
    /// <summary>
    /// Persists a new financial transaction.
    /// </summary>
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a transaction by identifier for a specific user.
    /// </summary>
    Task<Transaction?> GetByIdForUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets paginated transactions for a user.
    /// </summary>
    Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetPagedByUserIdAsync(
        Guid userId,
        int page,
        int pageSize,
        TransactionType? type,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets all transactions linked to a reference entity.
    /// </summary>
    Task<IReadOnlyList<Transaction>> GetByReferenceIdAsync(
        Guid referenceId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets all transactions for a user ordered chronologically.
    /// </summary>
    Task<IReadOnlyList<Transaction>> GetOrderedByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken);
}
