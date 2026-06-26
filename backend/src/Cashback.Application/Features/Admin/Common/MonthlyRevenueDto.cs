namespace Cashback.Application.Features.Admin.Common;

/// <summary>
/// Platform revenue total for a calendar month in admin dashboard responses.
/// </summary>
public sealed class MonthlyRevenueDto
{
    /// <summary>
    /// Calendar year.
    /// </summary>
    public int Year { get; init; }

    /// <summary>
    /// Calendar month.
    /// </summary>
    public int Month { get; init; }

    /// <summary>
    /// Platform revenue earned in the month.
    /// </summary>
    public decimal RevenueAmount { get; init; }
}
