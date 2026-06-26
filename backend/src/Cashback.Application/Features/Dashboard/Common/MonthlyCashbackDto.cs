namespace Cashback.Application.Features.Dashboard.Common;

/// <summary>
/// Cashback total for a calendar month.
/// </summary>
public sealed class MonthlyCashbackDto
{
    /// <summary>
    /// Calendar year of the month.
    /// </summary>
    public int Year { get; init; }

    /// <summary>
    /// Calendar month number.
    /// </summary>
    public int Month { get; init; }

    /// <summary>
    /// Total cashback amount for the month.
    /// </summary>
    public decimal CashbackAmount { get; init; }
}
