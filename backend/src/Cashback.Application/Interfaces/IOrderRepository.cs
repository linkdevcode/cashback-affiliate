using Cashback.Domain.Entities;
using Cashback.Domain.Enums;

namespace Cashback.Application.Interfaces;

/// <summary>
/// Persistence abstraction for affiliate orders.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Gets an order by network order identifier.
    /// </summary>
    Task<Order?> GetByNetworkOrderIdAsync(
        string networkOrderId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets an order by network order identifier for update operations.
    /// </summary>
    Task<Order?> GetByNetworkOrderIdForUpdateAsync(
        string networkOrderId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets an order by identifier for a specific user.
    /// </summary>
    Task<Order?> GetByIdForUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets a paginated list of orders for a user with optional filtering and sorting.
    /// </summary>
    Task<(IReadOnlyList<Order> Items, int TotalCount)> GetPagedByUserIdAsync(
        Guid userId,
        int page,
        int pageSize,
        OrderStatus? status,
        string sortBy,
        string sortDirection,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets aggregated order statistics for a user.
    /// </summary>
    Task<OrderUserSummary> GetUserSummaryAsync(
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets cashback totals grouped by order status for a user.
    /// </summary>
    Task<EarningsByStatus> GetEarningsByStatusAsync(
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets the most recent orders for a user.
    /// </summary>
    Task<IReadOnlyList<Order>> GetRecentByUserIdAsync(
        Guid userId,
        int count,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets monthly cashback totals for a user over a number of months.
    /// </summary>
    Task<IReadOnlyList<MonthlyCashbackTotal>> GetMonthlyCashbackTotalsAsync(
        Guid userId,
        int monthCount,
        CancellationToken cancellationToken);

    /// <summary>
    /// Persists a new order.
    /// </summary>
    Task AddAsync(Order order, CancellationToken cancellationToken);

    /// <summary>
    /// Persists changes to an existing order.
    /// </summary>
    Task UpdateAsync(Order order, CancellationToken cancellationToken);
}
