using Cashback.Domain.Enums;

namespace Cashback.Application.Features.Admin.Withdrawals.Common;

/// <summary>
/// Withdrawal summary for admin list responses.
/// </summary>
public sealed class AdminWithdrawalDto
{
    /// <summary>
    /// Withdrawal identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// User associated with the withdrawal.
    /// </summary>
    public AdminWithdrawalUserDto User { get; init; } = null!;

    /// <summary>
    /// Requested withdrawal amount.
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    /// Current withdrawal status value.
    /// </summary>
    public WithdrawalStatus Status { get; init; }

    /// <summary>
    /// Human-readable withdrawal status name.
    /// </summary>
    public string StatusName { get; init; } = null!;

    /// <summary>
    /// UTC timestamp when the withdrawal was requested.
    /// </summary>
    public DateTime RequestedAt { get; init; }
}
