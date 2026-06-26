using Cashback.Application.Features.Withdrawals.ApproveWithdrawal;
using Cashback.Application.Features.Withdrawals.CompleteWithdrawal;
using Cashback.Application.Features.Withdrawals.CreateWithdrawal;
using Cashback.Application.Features.Withdrawals.RejectWithdrawal;
using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Cashback.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Cashback.UnitTests.Features.Withdrawals;

/// <summary>
/// Unit tests for withdrawal request creation.
/// </summary>
public sealed class CreateWithdrawalHandlerTests
{
    /// <summary>
    /// Verifies a valid withdrawal request is created with a financial transaction.
    /// </summary>
    [Fact]
    public async Task Handle_ValidRequest_CreatesWithdrawalAndTransaction()
    {
        var userId = Guid.NewGuid();
        var currentUser = CreateCurrentUser(userId);
        var user = User.Create("user@example.com", "Test User", AuthProvider.Google, "google-1");

        var userRepository = new Mock<IUserRepository>();
        userRepository
            .Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var balanceService = new Mock<IBalanceService>();
        balanceService
            .Setup(service => service.GetOperationalAvailableBalanceAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(200_000m);

        var withdrawalRepository = new Mock<IWithdrawalRepository>();

        var handler = new CreateWithdrawalHandler(
            currentUser.Object,
            userRepository.Object,
            balanceService.Object,
            withdrawalRepository.Object,
            NullLogger<CreateWithdrawalHandler>.Instance);

        var result = await handler.Handle(
            new CreateWithdrawalCommand(
                100_000m,
                "Vietcombank",
                "0123456789",
                "Nguyen Van A"),
            CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(100_000m, result.Amount);
        Assert.Equal((int)WithdrawalStatus.Pending, result.Status);

        withdrawalRepository.Verify(
            repository => repository.CreateRequestWithTransactionAsync(
                It.Is<Withdrawal>(withdrawal =>
                    withdrawal.UserId == userId
                    && withdrawal.Amount == 100_000m
                    && withdrawal.Status == WithdrawalStatus.Pending),
                It.Is<Transaction>(transaction =>
                    transaction.UserId == userId
                    && transaction.Type == TransactionType.WithdrawalRequested
                    && transaction.Amount == -100_000m
                    && transaction.BalanceBefore == 200_000m
                    && transaction.BalanceAfter == 100_000m),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies insufficient balance prevents withdrawal creation.
    /// </summary>
    [Fact]
    public async Task Handle_InsufficientBalance_ThrowsBusinessRuleException()
    {
        var userId = Guid.NewGuid();
        var currentUser = CreateCurrentUser(userId);
        var user = User.Create("user@example.com", "Test User", AuthProvider.Google, "google-1");

        var userRepository = new Mock<IUserRepository>();
        userRepository
            .Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var balanceService = new Mock<IBalanceService>();
        balanceService
            .Setup(service => service.GetOperationalAvailableBalanceAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(80_000m);

        var withdrawalRepository = new Mock<IWithdrawalRepository>();

        var handler = new CreateWithdrawalHandler(
            currentUser.Object,
            userRepository.Object,
            balanceService.Object,
            withdrawalRepository.Object,
            NullLogger<CreateWithdrawalHandler>.Instance);

        await Assert.ThrowsAsync<BusinessRuleException>(() => handler.Handle(
            new CreateWithdrawalCommand(
                100_000m,
                "Vietcombank",
                "0123456789",
                "Nguyen Van A"),
            CancellationToken.None));
    }

    /// <summary>
    /// Verifies available balance from the balance service is enforced.
    /// </summary>
    [Fact]
    public async Task Handle_WithReducedAvailableBalance_ThrowsBusinessRuleException()
    {
        var userId = Guid.NewGuid();
        var currentUser = CreateCurrentUser(userId);
        var user = User.Create("user@example.com", "Test User", AuthProvider.Google, "google-1");

        var userRepository = new Mock<IUserRepository>();
        userRepository
            .Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var balanceService = new Mock<IBalanceService>();
        balanceService
            .Setup(service => service.GetOperationalAvailableBalanceAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(150_000m);

        var withdrawalRepository = new Mock<IWithdrawalRepository>();

        var handler = new CreateWithdrawalHandler(
            currentUser.Object,
            userRepository.Object,
            balanceService.Object,
            withdrawalRepository.Object,
            NullLogger<CreateWithdrawalHandler>.Instance);

        await Assert.ThrowsAsync<BusinessRuleException>(() => handler.Handle(
            new CreateWithdrawalCommand(
                200_000m,
                "Vietcombank",
                "0123456789",
                "Nguyen Van A"),
            CancellationToken.None));
    }

    /// <summary>
    /// Creates a mock current user service for the given user identifier.
    /// </summary>
    private static Mock<ICurrentUserService> CreateCurrentUser(Guid userId)
    {
        var currentUser = new Mock<ICurrentUserService>();
        currentUser.SetupGet(service => service.IsAuthenticated).Returns(true);
        currentUser.SetupGet(service => service.UserId).Returns(userId);

        return currentUser;
    }
}
