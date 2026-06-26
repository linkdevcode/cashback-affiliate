using Cashback.Domain.Enums;

namespace Cashback.Application.Features.Admin.Common;

/// <summary>
/// Recent withdrawal summary for admin dashboard activity widgets.
/// </summary>
public sealed class AdminRecentWithdrawalDto
{
    /// <summary>
    /// Withdrawal identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Email of the user who requested the withdrawal.
    /// </summary>
    public string UserEmail { get; init; } = null!;

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
