using Cashback.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Cashback.Infrastructure.Settings;

/// <summary>
/// Withdrawal configuration options.
/// </summary>
public sealed class WithdrawalOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string SectionName = "Withdrawal";

    /// <summary>
    /// Minimum amount allowed for a withdrawal request.
    /// </summary>
    public decimal MinimumWithdrawalAmount { get; init; } = 50_000m;
}

/// <summary>
/// Withdrawal settings backed by configuration options.
/// </summary>
public sealed class WithdrawalSettings : IWithdrawalSettings
{
    /// <summary>
    /// Initializes withdrawal settings from configuration.
    /// </summary>
    public WithdrawalSettings(IConfiguration configuration)
    {
        var options = configuration.GetSection(WithdrawalOptions.SectionName).Get<WithdrawalOptions>()
            ?? new WithdrawalOptions();

        MinimumWithdrawalAmount = options.MinimumWithdrawalAmount;
    }

    /// <inheritdoc/>
    public decimal MinimumWithdrawalAmount { get; }
}
