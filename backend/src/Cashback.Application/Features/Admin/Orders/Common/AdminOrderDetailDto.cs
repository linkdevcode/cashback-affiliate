using Cashback.Domain.Enums;

namespace Cashback.Application.Features.Admin.Orders.Common;

/// <summary>
/// Detailed order representation for admin responses.
/// </summary>
public sealed class AdminOrderDetailDto
{
    /// <summary>
    /// Order identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Provider order code.
    /// </summary>
    public string OrderCode { get; init; } = null!;

    /// <summary>
    /// User associated with the order.
    /// </summary>
    public AdminOrderUserDto User { get; init; } = null!;

    /// <summary>
    /// Merchant where the order was placed.
    /// </summary>
    public string? Merchant { get; init; }

    /// <summary>
    /// Total order value reported by the provider.
    /// </summary>
    public decimal? OrderAmount { get; init; }

    /// <summary>
    /// Commission amount received from the provider.
    /// </summary>
    public decimal CommissionAmount { get; init; }

    /// <summary>
    /// Cashback amount credited to the user.
    /// </summary>
    public decimal CashbackAmount { get; init; }

    /// <summary>
    /// Platform profit retained from the commission.
    /// </summary>
    public decimal PlatformProfit { get; init; }

    /// <summary>
    /// Current order status value.
    /// </summary>
    public OrderStatus Status { get; init; }

    /// <summary>
    /// Human-readable order status name.
    /// </summary>
    public string StatusName { get; init; } = null!;

    /// <summary>
    /// UTC timestamp when the order was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
