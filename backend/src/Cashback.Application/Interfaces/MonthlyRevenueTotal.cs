namespace Cashback.Application.Interfaces;

/// <summary>
/// Platform revenue total for a calendar month.
/// </summary>
public sealed record MonthlyRevenueTotal(
    int Year,
    int Month,
    decimal RevenueAmount);
