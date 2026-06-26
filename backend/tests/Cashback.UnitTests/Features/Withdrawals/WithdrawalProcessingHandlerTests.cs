using Cashback.Application.Features.Withdrawals.ApproveWithdrawal;
using Cashback.Application.Features.Withdrawals.CompleteWithdrawal;
using Cashback.Application.Features.Withdrawals.RejectWithdrawal;
using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Cashback.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Cashback.UnitTests.Features.Withdrawals;

/// <summary>
/// Unit tests for admin withdrawal processing handlers.
/// </summary>
public sealed class WithdrawalProcessingHandlerTests
{
    /// <summary>
    /// Verifies approving a pending withdrawal records a transaction and audit log.
    /// </summary>
    [Fact]
    public async Task ApproveWithdrawal_ValidRequest_UpdatesStatusAndCreatesTransaction()
    {
        var adminUserId = Guid.NewGuid();
        var withdrawal = CreatePendingWithdrawal();

        var currentUser = CreateAdminUser(adminUserId);
        var withdrawalRepository = new Mock<IWithdrawalRepository>();
        withdrawalRepository
            .Setup(repository => repository.GetByIdForUpdateAsync(withdrawal.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(withdrawal);

        var balanceService = new Mock<IBalanceService>();
        balanceService
            .Setup(service => service.GetOperationalAvailableBalanceAsync(withdrawal.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(50_000m);

        var auditLogService = new Mock<IAuditLogService>();

        var handler = new ApproveWithdrawalHandler(
            currentUser.Object,
            withdrawalRepository.Object,
            balanceService.Object,
            auditLogService.Object,
            NullLogger<ApproveWithdrawalHandler>.Instance);

        var result = await handler.Handle(
            new ApproveWithdrawalCommand(withdrawal.Id),
            CancellationToken.None);

        Assert.Equal((int)WithdrawalStatus.Approved, result.Status);
        Assert.Equal(WithdrawalStatus.Approved, withdrawal.Status);

        withdrawalRepository.Verify(
            repository => repository.ProcessWithdrawalUpdateAsync(
                withdrawal,
                It.Is<Transaction>(transaction =>
                    transaction.Type == TransactionType.WithdrawalApproved
                    && transaction.Amount == 0m),
                It.IsAny<CancellationToken>()),
            Times.Once);

        auditLogService.Verify(
            service => service.LogWithdrawalActionAsync(
                adminUserId,
                withdrawal.Id,
                AuditAction.WithdrawalApproved,
                WithdrawalStatus.Pending,
                WithdrawalStatus.Approved,
                null,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies rejecting a pending withdrawal restores balance in the transaction trail.
    /// </summary>
    [Fact]
    public async Task RejectWithdrawal_ValidRequest_RestoresBalanceAndCreatesTransaction()
    {
        var adminUserId = Guid.NewGuid();
        var withdrawal = CreatePendingWithdrawal();

        var currentUser = CreateAdminUser(adminUserId);
        var withdrawalRepository = new Mock<IWithdrawalRepository>();
        withdrawalRepository
            .Setup(repository => repository.GetByIdForUpdateAsync(withdrawal.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(withdrawal);

        var balanceService = new Mock<IBalanceService>();
        balanceService
            .Setup(service => service.GetOperationalAvailableBalanceAsync(withdrawal.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(50_000m);

        var auditLogService = new Mock<IAuditLogService>();

        var handler = new RejectWithdrawalHandler(
            currentUser.Object,
            withdrawalRepository.Object,
            balanceService.Object,
            auditLogService.Object,
            NullLogger<RejectWithdrawalHandler>.Instance);

        var result = await handler.Handle(
            new RejectWithdrawalCommand(withdrawal.Id, "Invalid bank account"),
            CancellationToken.None);

        Assert.Equal((int)WithdrawalStatus.Rejected, result.Status);
        Assert.Equal(WithdrawalStatus.Rejected, withdrawal.Status);
        Assert.NotNull(withdrawal.ProcessedAt);

        withdrawalRepository.Verify(
            repository => repository.ProcessWithdrawalUpdateAsync(
                withdrawal,
                It.Is<Transaction>(transaction =>
                    transaction.Type == TransactionType.WithdrawalRejected
                    && transaction.Amount == withdrawal.Amount
                    && transaction.BalanceAfter == 150_000m),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies completing an approved withdrawal records a completion transaction.
    /// </summary>
    [Fact]
    public async Task CompleteWithdrawal_ValidRequest_UpdatesStatusAndCreatesTransaction()
    {
        var adminUserId = Guid.NewGuid();
        var withdrawal = CreatePendingWithdrawal();
        withdrawal.Approve();

        var currentUser = CreateAdminUser(adminUserId);
        var withdrawalRepository = new Mock<IWithdrawalRepository>();
        withdrawalRepository
            .Setup(repository => repository.GetByIdForUpdateAsync(withdrawal.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(withdrawal);

        var balanceService = new Mock<IBalanceService>();
        balanceService
            .Setup(service => service.GetOperationalAvailableBalanceAsync(withdrawal.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(50_000m);

        var auditLogService = new Mock<IAuditLogService>();

        var handler = new CompleteWithdrawalHandler(
            currentUser.Object,
            withdrawalRepository.Object,
            balanceService.Object,
            auditLogService.Object,
            NullLogger<CompleteWithdrawalHandler>.Instance);

        var result = await handler.Handle(
            new CompleteWithdrawalCommand(withdrawal.Id),
            CancellationToken.None);

        Assert.Equal((int)WithdrawalStatus.Completed, result.Status);
        Assert.Equal(WithdrawalStatus.Completed, withdrawal.Status);
        Assert.NotNull(withdrawal.ProcessedAt);

        withdrawalRepository.Verify(
            repository => repository.ProcessWithdrawalUpdateAsync(
                withdrawal,
                It.Is<Transaction>(transaction =>
                    transaction.Type == TransactionType.WithdrawalCompleted),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies non-admin users cannot approve withdrawals.
    /// </summary>
    [Fact]
    public async Task ApproveWithdrawal_NonAdmin_ThrowsForbiddenException()
    {
        var currentUser = new Mock<ICurrentUserService>();
        currentUser.SetupGet(service => service.IsAuthenticated).Returns(true);
        currentUser.SetupGet(service => service.UserId).Returns(Guid.NewGuid());
        currentUser.SetupGet(service => service.Role).Returns(UserRole.User);

        var handler = new ApproveWithdrawalHandler(
            currentUser.Object,
            Mock.Of<IWithdrawalRepository>(),
            Mock.Of<IBalanceService>(),
            Mock.Of<IAuditLogService>(),
            NullLogger<ApproveWithdrawalHandler>.Instance);

        await Assert.ThrowsAsync<ForbiddenException>(() => handler.Handle(
            new ApproveWithdrawalCommand(Guid.NewGuid()),
            CancellationToken.None));
    }

    /// <summary>
    /// Creates a pending withdrawal for handler tests.
    /// </summary>
    private static Withdrawal CreatePendingWithdrawal()
    {
        return Withdrawal.Create(
            Guid.NewGuid(),
            100_000m,
            "Vietcombank",
            "0123456789",
            "Nguyen Van A");
    }

    /// <summary>
    /// Creates a mock admin current user service.
    /// </summary>
    private static Mock<ICurrentUserService> CreateAdminUser(Guid adminUserId)
    {
        var currentUser = new Mock<ICurrentUserService>();
        currentUser.SetupGet(service => service.IsAuthenticated).Returns(true);
        currentUser.SetupGet(service => service.UserId).Returns(adminUserId);
        currentUser.SetupGet(service => service.Role).Returns(UserRole.Admin);

        return currentUser;
    }
}
