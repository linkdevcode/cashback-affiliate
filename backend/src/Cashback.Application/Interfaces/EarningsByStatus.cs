namespace Cashback.Application.Interfaces;

/// <summary>
/// Cashback totals grouped by order status for a user.
/// </summary>
public sealed record EarningsByStatus(
    decimal PendingCashback,
    decimal ApprovedCashback,
    decimal RejectedCashback);
