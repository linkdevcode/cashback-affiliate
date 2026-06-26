using Cashback.Domain.Enums;

namespace Cashback.Application.Features.Admin.Withdrawals.Common;

/// <summary>
/// Detailed withdrawal information for admin management.
/// </summary>
public sealed class AdminWithdrawalDetailDto
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
    /// Name of the destination bank.
    /// </summary>
    public string BankName { get; init; } = null!;

    /// <summary>
    /// Destination bank account number.
    /// </summary>
    public string BankAccountNumber { get; init; } = null!;

    /// <summary>
    /// Name of the bank account holder.
    /// </summary>
    public string BankAccountHolder { get; init; } = null!;

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
