namespace Cashback.Api.Models.Withdrawals;

/// <summary>
/// Request body for creating a withdrawal request.
/// </summary>
public sealed class CreateWithdrawalRequest
{
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
    public string BankAccountName { get; init; } = null!;
}
