namespace Cashback.Application.Features.Orders.GetOrderSummary;

/// <summary>
/// Aggregated order statistics for the authenticated user.
/// </summary>
public sealed class GetOrderSummaryResponse
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

    /// <summary>
    /// Cashback from orders awaiting approval.
    /// </summary>
    public decimal PendingCashback { get; init; }

    /// <summary>
    /// Cashback from approved orders.
    /// </summary>
    public decimal ApprovedCashback { get; init; }

    /// <summary>
    /// Cashback from rejected orders.
    /// </summary>
    public decimal RejectedCashback { get; init; }
}
