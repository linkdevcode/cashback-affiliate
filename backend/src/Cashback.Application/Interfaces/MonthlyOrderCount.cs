namespace Cashback.Application.Interfaces;

/// <summary>
/// Order count for a calendar month.
/// </summary>
public sealed record MonthlyOrderCount(
    int Year,
    int Month,
    int OrderCount);
