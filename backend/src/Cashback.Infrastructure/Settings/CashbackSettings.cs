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
    public decimal Percentage { get; init; } = 70m;
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

        CashbackPercentage = options.Percentage;
    }

    /// <inheritdoc/>
    public decimal CashbackPercentage { get; }
}
