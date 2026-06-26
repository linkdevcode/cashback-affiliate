using Cashback.Domain.Enums;

namespace Cashback.Application.Features.Admin.Orders.Common;

/// <summary>
/// Order summary for admin order list responses.
/// </summary>
public sealed class AdminOrderDto
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
    /// Commission amount received from the provider.
    /// </summary>
    public decimal CommissionAmount { get; init; }

    /// <summary>
    /// Cashback amount credited to the user.
    /// </summary>
    public decimal CashbackAmount { get; init; }

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
