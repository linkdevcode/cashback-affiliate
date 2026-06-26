using Cashback.Application.Features.Earnings.Common;
using Cashback.Application.Interfaces;
using Cashback.Application.Services;
using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Moq;

namespace Cashback.UnitTests.Services;

/// <summary>
/// Unit tests for financial balance calculations and audit validation.
/// </summary>
public sealed class BalanceServiceTests
{
    /// <summary>
    /// Verifies available balance uses the audit verification formula.
    /// </summary>
    [Fact]
    public async Task GetAvailableBalanceAsync_UsesVerificationFormula()
    {
        var userId = Guid.NewGuid();
        var service = CreateService(
            userId,
            approvedCashback: 500_000m,
            pendingWithdrawals: 100_000m,
            approvedWithdrawals: 50_000m,
            completedWithdrawals: 120_000m);

        var balance = await service.GetAvailableBalanceAsync(userId, CancellationToken.None);

        Assert.Equal(280_000m, balance);
    }

    /// <summary>
    /// Verifies operational balance subtracts approved withdrawal reservations.
    /// </summary>
    [Fact]
    public async Task GetOperationalAvailableBalanceAsync_SubtractsApprovedWithdrawals()
    {
        var userId = Guid.NewGuid();
        var service = CreateService(
            userId,
            approvedCashback: 500_000m,
            pendingWithdrawals: 100_000m,
            approvedWithdrawals: 50_000m,
            completedWithdrawals: 120_000m);

        var balance = await service.GetOperationalAvailableBalanceAsync(userId, CancellationToken.None);

        Assert.Equal(230_000m, balance);
    }

    /// <summary>
    /// Verifies a valid transaction chain passes validation.
    /// </summary>
    [Fact]
    public async Task ValidateTransactionsAsync_ValidChain_ReturnsValidResult()
    {
        var userId = Guid.NewGuid();
        var transactions = new List<Transaction>
        {
            CreateTransaction(userId, 100_000m, 0m, 100_000m),
            CreateTransaction(userId, -30_000m, 100_000m, 70_000m)
        };

        var service = CreateService(userId, transactions: transactions);

        var result = await service.ValidateTransactionsAsync(userId, CancellationToken.None);

        Assert.True(result.IsValid);
        Assert.False(result.HasNegativeBalance);
        Assert.Equal(70_000m, result.LedgerBalance);
    }

    /// <summary>
    /// Verifies invalid balance transitions are detected.
    /// </summary>
    [Fact]
    public async Task ValidateTransactionsAsync_InvalidBalanceTransition_ReturnsErrors()
    {
        var userId = Guid.NewGuid();
        var transactions = new List<Transaction>
        {
            CreateTransaction(userId, 100_000m, 0m, 90_000m)
        };

        var service = CreateService(userId, transactions: transactions);

        var result = await service.ValidateTransactionsAsync(userId, CancellationToken.None);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.Contains("invalid balance transition"));
    }

    /// <summary>
    /// Verifies negative balances are detected during validation.
    /// </summary>
    [Fact]
    public async Task ValidateTransactionsAsync_NegativeBalance_ReturnsHasNegativeBalance()
    {
        var userId = Guid.NewGuid();
        var transactions = new List<Transaction>
        {
            CreateTransaction(userId, -50_000m, 30_000m, -20_000m)
        };

        var service = CreateService(userId, transactions: transactions);

        var result = await service.ValidateTransactionsAsync(userId, CancellationToken.None);

        Assert.False(result.IsValid);
        Assert.True(result.HasNegativeBalance);
    }

    /// <summary>
    /// Verifies balance audit reports consistency when ledger matches operational balance.
    /// </summary>
    [Fact]
    public async Task GetBalanceAuditAsync_LedgerMatchesOperational_ReturnsConsistentAudit()
    {
        var userId = Guid.NewGuid();
        var transactions = new List<Transaction>
        {
            CreateTransaction(userId, -80_000m, 300_000m, 220_000m)
        };

        var service = CreateService(
            userId,
            approvedCashback: 300_000m,
            pendingWithdrawals: 80_000m,
            approvedWithdrawals: 0m,
            completedWithdrawals: 0m,
            transactions: transactions);

        var audit = await service.GetBalanceAuditAsync(userId, CancellationToken.None);

        Assert.Equal(220_000m, audit.AvailableBalance);
        Assert.Equal(220_000m, audit.OperationalAvailableBalance);
        Assert.True(audit.IsConsistent);
        Assert.False(audit.HasNegativeBalance);
    }

    /// <summary>
    /// Verifies balance audit detects negative available balance.
    /// </summary>
    [Fact]
    public async Task GetBalanceAuditAsync_NegativeAvailableBalance_FlagsIssue()
    {
        var userId = Guid.NewGuid();
        var service = CreateService(
            userId,
            approvedCashback: 100_000m,
            pendingWithdrawals: 150_000m,
            approvedWithdrawals: 0m,
            completedWithdrawals: 0m);

        var audit = await service.GetBalanceAuditAsync(userId, CancellationToken.None);

        Assert.True(audit.HasNegativeBalance);
        Assert.Equal(-50_000m, audit.AvailableBalance);
    }

    /// <summary>
    /// Verifies empty transaction chains pass validation.
    /// </summary>
    [Fact]
    public async Task ValidateTransactionsAsync_EmptyChain_ReturnsValidResult()
    {
        var userId = Guid.NewGuid();
        var service = CreateService(userId, transactions: []);

        var result = await service.ValidateTransactionsAsync(userId, CancellationToken.None);

        Assert.True(result.IsValid);
        Assert.False(result.HasNegativeBalance);
        Assert.Null(result.LedgerBalance);
    }

    /// <summary>
    /// Creates a balance service with mocked dependencies.
    /// </summary>
    private static BalanceService CreateService(
        Guid userId,
        decimal approvedCashback = 0m,
        decimal pendingWithdrawals = 0m,
        decimal approvedWithdrawals = 0m,
        decimal completedWithdrawals = 0m,
        IReadOnlyList<Transaction>? transactions = null)
    {
        var earningsService = new Mock<IEarningsService>();
        earningsService
            .Setup(service => service.GetUserEarningsAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EarningsSummaryDto
            {
                ApprovedCashback = approvedCashback,
                PendingCashback = 0m,
                RejectedCashback = 0m
            });

        var withdrawalRepository = new Mock<IWithdrawalRepository>();
        withdrawalRepository
            .Setup(repository => repository.GetTotalAmountByUserIdAndStatusesAsync(
                userId,
                It.Is<IReadOnlyList<WithdrawalStatus>>(statuses => statuses.Contains(WithdrawalStatus.Pending)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(pendingWithdrawals);
        withdrawalRepository
            .Setup(repository => repository.GetTotalAmountByUserIdAndStatusesAsync(
                userId,
                It.Is<IReadOnlyList<WithdrawalStatus>>(statuses => statuses.Contains(WithdrawalStatus.Approved)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(approvedWithdrawals);
        withdrawalRepository
            .Setup(repository => repository.GetTotalCompletedAmountByUserIdAsync(
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(completedWithdrawals);

        var transactionRepository = new Mock<ITransactionRepository>();
        transactionRepository
            .Setup(repository => repository.GetOrderedByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactions ?? []);

        return new BalanceService(
            earningsService.Object,
            withdrawalRepository.Object,
            transactionRepository.Object);
    }

    /// <summary>
    /// Creates a withdrawal transaction for validation tests.
    /// </summary>
    private static Transaction CreateTransaction(
        Guid userId,
        decimal amount,
        decimal balanceBefore,
        decimal balanceAfter)
    {
        return Transaction.Create(
            userId,
            TransactionType.WithdrawalRequested,
            amount,
            balanceBefore,
            balanceAfter,
            referenceId: Guid.NewGuid(),
            description: "Test transaction");
    }
}
