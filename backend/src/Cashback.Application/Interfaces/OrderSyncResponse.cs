using Cashback.Domain.Enums;

namespace Cashback.Application.Interfaces;

/// <summary>
/// Result of synchronizing an order from a webhook event.
/// </summary>
public sealed class OrderSyncResponse
{
    /// <summary>
    /// Identifier of the synchronized order.
    /// </summary>
    public Guid OrderId { get; init; }

    /// <summary>
    /// Order status after synchronization.
    /// </summary>
    public OrderStatus Status { get; init; }

    /// <summary>
    /// Indicates whether a new order was created.
    /// </summary>
    public bool WasCreated { get; init; }

    /// <summary>
    /// Indicates whether an existing order was updated.
    /// </summary>
    public bool WasUpdated { get; init; }

    /// <summary>
    /// Previous order status before synchronization.
    /// </summary>
    public OrderStatus? PreviousStatus { get; init; }
}
