namespace Cashback.Application.Interfaces;

/// <summary>
/// Cashback total for a calendar month.
/// </summary>
public sealed record MonthlyCashbackTotal(
    int Year,
    int Month,
    decimal CashbackAmount);
