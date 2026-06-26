using Cashback.Domain.Common;
using Cashback.Domain.Enums;
using Cashback.Domain.Exceptions;

namespace Cashback.Domain.Entities;

/// <summary>
/// User request to withdraw available balance to a bank account.
/// </summary>
public class Withdrawal : BaseEntity
{
    /// <summary>
    /// User who submitted the withdrawal request.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Requested withdrawal amount.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Name of the destination bank.
    /// </summary>
    public string BankName { get; private set; } = null!;

    /// <summary>
    /// Destination bank account number.
    /// </summary>
    public string BankAccountNumber { get; private set; } = null!;

    /// <summary>
    /// Name of the bank account holder.
    /// </summary>
    public string BankAccountHolder { get; private set; } = null!;

    /// <summary>
    /// Current processing status of the withdrawal.
    /// </summary>
    public WithdrawalStatus Status { get; private set; }

    /// <summary>
    /// UTC timestamp when the withdrawal was requested.
    /// </summary>
    public DateTime RequestedAt { get; private set; }

    /// <summary>
    /// UTC timestamp when the withdrawal was processed.
    /// </summary>
    public DateTime? ProcessedAt { get; private set; }

    /// <summary>
    /// User associated with the withdrawal.
    /// </summary>
    public User User { get; private set; } = null!;

    /// <summary>
    /// Required by Entity Framework.
    /// </summary>
    private Withdrawal()
    {
    }

    /// <summary>
    /// Creates a new pending withdrawal request.
    /// </summary>
    public static Withdrawal Create(
        Guid userId,
        decimal amount,
        string bankName,
        string bankAccountNumber,
        string bankAccountHolder)
    {
        return new Withdrawal
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Amount = amount,
            BankName = bankName,
            BankAccountNumber = bankAccountNumber,
            BankAccountHolder = bankAccountHolder,
            Status = WithdrawalStatus.Pending,
            RequestedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Approves a pending withdrawal request.
    /// </summary>
    public void Approve()
    {
        ApplyStatusTransition(WithdrawalStatus.Approved);
    }

    /// <summary>
    /// Rejects a pending withdrawal request.
    /// </summary>
    public void Reject()
    {
        ApplyStatusTransition(WithdrawalStatus.Rejected);
        ProcessedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks an approved withdrawal as completed.
    /// </summary>
    public void Complete()
    {
        ApplyStatusTransition(WithdrawalStatus.Completed);
        ProcessedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Applies an allowed withdrawal status transition.
    /// </summary>
    private void ApplyStatusTransition(WithdrawalStatus newStatus)
    {
        if (Status == newStatus)
        {
            return;
        }

        var isAllowed = (Status, newStatus) switch
        {
            (WithdrawalStatus.Pending, WithdrawalStatus.Approved) => true,
            (WithdrawalStatus.Pending, WithdrawalStatus.Rejected) => true,
            (WithdrawalStatus.Approved, WithdrawalStatus.Completed) => true,
            _ => false
        };

        if (!isAllowed)
        {
            throw new BusinessRuleException(
                $"Withdrawal status cannot transition from {Status} to {newStatus}.");
        }

        Status = newStatus;
    }
}
