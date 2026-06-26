using Cashback.Domain.Enums;

namespace Cashback.Application.Features.Admin.Common;

/// <summary>
/// Order count for a calendar month in admin dashboard responses.
/// </summary>
public sealed class MonthlyOrderCountDto
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
    /// Number of orders created in the month.
    /// </summary>
    public int OrderCount { get; init; }
}
