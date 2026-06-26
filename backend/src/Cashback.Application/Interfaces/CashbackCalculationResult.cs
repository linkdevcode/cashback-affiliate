namespace Cashback.Application.Interfaces;

/// <summary>
/// Result of a cashback calculation from commission amount.
/// </summary>
public sealed record CashbackCalculationResult(
    decimal CashbackAmount,
    decimal PlatformRevenue);
