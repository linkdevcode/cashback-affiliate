namespace Cashback.Application.Features.Earnings.Common;

/// <summary>
/// User earnings summary grouped by order status.
/// </summary>
public sealed class EarningsSummaryDto
{
    /// <summary>
    /// Total cashback from orders awaiting approval.
    /// </summary>
    public decimal PendingCashback { get; init; }

    /// <summary>
    /// Total cashback from approved orders.
    /// </summary>
    public decimal ApprovedCashback { get; init; }

    /// <summary>
    /// Total cashback from rejected orders.
    /// </summary>
    public decimal RejectedCashback { get; init; }
}
