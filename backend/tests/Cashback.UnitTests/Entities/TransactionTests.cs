using Cashback.Domain.Entities;
using Cashback.Domain.Enums;

namespace Cashback.UnitTests.Entities;

/// <summary>
/// Unit tests for financial transaction entity behavior.
/// </summary>
public sealed class TransactionTests
{
    /// <summary>
    /// Verifies a cashback transaction records balance snapshots correctly.
    /// </summary>
    [Fact]
    public void Create_CashbackEarned_RecordsBalanceSnapshots()
    {
        var userId = Guid.NewGuid();
        var orderId = Guid.NewGuid();

        var transaction = Transaction.Create(
            userId,
            TransactionType.CashbackEarned,
            amount: 80_000m,
            balanceBefore: 20_000m,
            balanceAfter: 100_000m,
            referenceId: orderId,
            description: "Order approved");

        Assert.NotEqual(Guid.Empty, transaction.Id);
        Assert.Equal(userId, transaction.UserId);
        Assert.Equal(TransactionType.CashbackEarned, transaction.Type);
        Assert.Equal(80_000m, transaction.Amount);
        Assert.Equal(20_000m, transaction.BalanceBefore);
        Assert.Equal(100_000m, transaction.BalanceAfter);
        Assert.Equal(orderId, transaction.ReferenceId);
        Assert.Equal("Order approved", transaction.Description);
        Assert.True(transaction.CreatedAt <= DateTime.UtcNow);
    }

    /// <summary>
    /// Verifies withdrawal request transactions can be created without a reference.
    /// </summary>
    [Fact]
    public void Create_WithdrawalRequested_AllowsOptionalReference()
    {
        var userId = Guid.NewGuid();

        var transaction = Transaction.Create(
            userId,
            TransactionType.WithdrawalRequested,
            amount: 50_000m,
            balanceBefore: 100_000m,
            balanceAfter: 50_000m);

        Assert.Null(transaction.ReferenceId);
        Assert.Null(transaction.Description);
        Assert.Equal(TransactionType.WithdrawalRequested, transaction.Type);
    }

    /// <summary>
    /// Verifies rejected withdrawal transactions restore balance in the audit trail.
    /// </summary>
    [Fact]
    public void Create_WithdrawalRejected_RecordsRestoredBalance()
    {
        var userId = Guid.NewGuid();
        var withdrawalId = Guid.NewGuid();

        var transaction = Transaction.Create(
            userId,
            TransactionType.WithdrawalRejected,
            amount: 50_000m,
            balanceBefore: 50_000m,
            balanceAfter: 100_000m,
            referenceId: withdrawalId,
            description: "Withdrawal rejected");

        Assert.Equal(TransactionType.WithdrawalRejected, transaction.Type);
        Assert.Equal(withdrawalId, transaction.ReferenceId);
        Assert.Equal(100_000m, transaction.BalanceAfter);
    }
}
