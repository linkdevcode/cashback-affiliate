namespace Cashback.Application.Features.Withdrawals.CreateWithdrawal;

/// <summary>
/// Response returned after a withdrawal request is created.
/// </summary>
public sealed class CreateWithdrawalResponse
{
    /// <summary>
    /// Identifier of the created withdrawal request.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Requested withdrawal amount.
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    /// Current processing status of the withdrawal.
    /// </summary>
    public int Status { get; init; }

    /// <summary>
    /// UTC timestamp when the withdrawal was requested.
    /// </summary>
    public DateTime RequestedAt { get; init; }
}
