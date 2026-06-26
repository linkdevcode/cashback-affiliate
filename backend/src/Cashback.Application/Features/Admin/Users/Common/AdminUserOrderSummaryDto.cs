namespace Cashback.Application.Features.Admin.Users.Common;

/// <summary>
/// Order summary for admin user detail responses.
/// </summary>
public sealed class AdminUserOrderSummaryDto
{
    /// <summary>
    /// Total number of orders.
    /// </summary>
    public int TotalOrders { get; init; }

    /// <summary>
    /// Number of pending orders.
    /// </summary>
    public int PendingOrders { get; init; }

    /// <summary>
    /// Number of approved orders.
    /// </summary>
    public int ApprovedOrders { get; init; }

    /// <summary>
    /// Number of rejected orders.
    /// </summary>
    public int RejectedOrders { get; init; }

    /// <summary>
    /// Total commission amount across all orders.
    /// </summary>
    public decimal TotalCommission { get; init; }

    /// <summary>
    /// Total cashback amount across all orders.
    /// </summary>
    public decimal TotalCashback { get; init; }
}
