using Cashback.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Cashback.Infrastructure.Settings;

/// <summary>
/// Cashback configuration options.
/// </summary>
public sealed class CashbackOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string SectionName = "Cashback";

    /// <summary>
    /// Percentage of commission paid to users as cashback.
    /// </summary>
    public decimal CashbackPercentage { get; init; } = 80m;

    /// <summary>
    /// Percentage of commission retained by the platform.
    /// </summary>
    public decimal PlatformCommissionPercentage { get; init; } = 20m;
}

/// <summary>
/// Cashback settings backed by configuration options.
/// </summary>
public sealed class CashbackSettings : ICashbackSettings
{
    /// <summary>
    /// Initializes cashback settings from configuration.
    /// </summary>
    public CashbackSettings(IConfiguration configuration)
    {
        var options = configuration.GetSection(CashbackOptions.SectionName).Get<CashbackOptions>()
            ?? new CashbackOptions();

        CashbackPercentage = options.CashbackPercentage;
        PlatformCommissionPercentage = options.PlatformCommissionPercentage;
    }

    /// <inheritdoc/>
    public decimal CashbackPercentage { get; }

    /// <inheritdoc/>
    public decimal PlatformCommissionPercentage { get; }
}
