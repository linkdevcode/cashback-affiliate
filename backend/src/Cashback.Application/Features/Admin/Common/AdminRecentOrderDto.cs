using Cashback.Domain.Enums;

namespace Cashback.Application.Features.Admin.Common;

/// <summary>
/// Recent order summary for admin dashboard activity widgets.
/// </summary>
public sealed class AdminRecentOrderDto
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
    /// Email of the user associated with the order.
    /// </summary>
    public string UserEmail { get; init; } = null!;

    /// <summary>
    /// Current order status value.
    /// </summary>
    public OrderStatus Status { get; init; }

    /// <summary>
    /// Human-readable order status name.
    /// </summary>
    public string StatusName { get; init; } = null!;

    /// <summary>
    /// Cashback amount credited to the user.
    /// </summary>
    public decimal CashbackAmount { get; init; }

    /// <summary>
    /// UTC timestamp when the order was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
