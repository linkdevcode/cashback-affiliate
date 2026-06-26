namespace Cashback.Application.Interfaces;

/// <summary>
/// Business configuration for cashback calculations.
/// </summary>
public interface ICashbackSettings
{
    /// <summary>
    /// Percentage of commission paid to users as cashback.
    /// </summary>
    decimal CashbackPercentage { get; }

    /// <summary>
    /// Percentage of commission retained by the platform.
    /// </summary>
    decimal PlatformCommissionPercentage { get; }
}
