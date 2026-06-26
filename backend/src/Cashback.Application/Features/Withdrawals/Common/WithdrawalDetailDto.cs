using Cashback.Domain.Enums;

namespace Cashback.Application.Features.Withdrawals.Common;

/// <summary>
/// Detailed representation of a withdrawal request.
/// </summary>
public sealed class WithdrawalDetailDto
{
    /// <summary>
    /// Withdrawal identifier.
    /// </summary>
    public Guid Id { get; init; }

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

    /// <summary>
    /// UTC timestamp when the withdrawal was processed.
    /// </summary>
    public DateTime? ProcessedAt { get; init; }
}
