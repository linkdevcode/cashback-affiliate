using Cashback.Domain.Entities;

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
    /// Persists a new order.
    /// </summary>
    Task AddAsync(Order order, CancellationToken cancellationToken);

    /// <summary>
    /// Persists changes to an existing order.
    /// </summary>
    Task UpdateAsync(Order order, CancellationToken cancellationToken);
}
