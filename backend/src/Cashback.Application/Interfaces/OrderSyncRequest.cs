using Cashback.Domain.Enums;

namespace Cashback.Application.Interfaces;

/// <summary>
/// Input for synchronizing an order from a webhook event.
/// </summary>
public sealed class OrderSyncRequest
{
    /// <summary>
    /// Provider network order identifier.
    /// </summary>
    public string NetworkOrderId { get; init; } = string.Empty;

    /// <summary>
    /// Tracking parameter used to resolve the order owner.
    /// </summary>
    public string Sub1 { get; init; } = string.Empty;

    /// <summary>
    /// Commission amount reported by the provider.
    /// </summary>
    public decimal CommissionAmount { get; init; }

    /// <summary>
    /// Target order status after synchronization.
    /// </summary>
    public OrderStatus TargetStatus { get; init; }

    /// <summary>
    /// Webhook event identifier associated with the synchronization.
    /// </summary>
    public Guid WebhookEventId { get; init; }
}
