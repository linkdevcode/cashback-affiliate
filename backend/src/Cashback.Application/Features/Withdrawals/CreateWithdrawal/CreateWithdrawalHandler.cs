using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Cashback.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cashback.Application.Features.Withdrawals.CreateWithdrawal;

/// <summary>
/// Handles creation of a withdrawal request with balance reservation.
/// </summary>
public sealed class CreateWithdrawalHandler
    : IRequestHandler<CreateWithdrawalCommand, CreateWithdrawalResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly IWithdrawalRepository _withdrawalRepository;
    private readonly IBalanceService _balanceService;
    private readonly ILogger<CreateWithdrawalHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the create withdrawal handler.
    /// </summary>
    public CreateWithdrawalHandler(
        ICurrentUserService currentUserService,
        IUserRepository userRepository,
        IBalanceService balanceService,
        IWithdrawalRepository withdrawalRepository,
        ILogger<CreateWithdrawalHandler> logger)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _balanceService = balanceService;
        _withdrawalRepository = withdrawalRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<CreateWithdrawalResponse> Handle(
        CreateWithdrawalCommand request,
        CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException("User not found.");
        }

        if (user.Status == UserStatus.Suspended)
        {
            throw new ForbiddenException("Your account has been suspended.");
        }

        var availableBalance = await _balanceService.GetOperationalAvailableBalanceAsync(
            userId,
            cancellationToken);

        if (request.Amount > availableBalance)
        {
            throw new BusinessRuleException("Available balance is insufficient.");
        }

        var withdrawal = Withdrawal.Create(
            userId,
            request.Amount,
            request.BankName.Trim(),
            request.BankAccountNumber.Trim(),
            request.BankAccountHolder.Trim());

        var transaction = Transaction.Create(
            userId,
            TransactionType.WithdrawalRequested,
            amount: -request.Amount,
            balanceBefore: availableBalance,
            balanceAfter: availableBalance - request.Amount,
            referenceId: withdrawal.Id,
            description: "Withdrawal requested");

        await _withdrawalRepository.CreateRequestWithTransactionAsync(
            withdrawal,
            transaction,
            cancellationToken);

        _logger.LogInformation(
            "Withdrawal {WithdrawalId} requested by user {UserId} for amount {Amount}",
            withdrawal.Id,
            userId,
            request.Amount);

        return new CreateWithdrawalResponse
        {
            Id = withdrawal.Id,
            Amount = withdrawal.Amount,
            Status = (int)withdrawal.Status,
            RequestedAt = withdrawal.RequestedAt
        };
    }

    /// <summary>
    /// Resolves the authenticated user identifier from the request context.
    /// </summary>
    private Guid GetAuthenticatedUserId()
    {
        if (!_currentUserService.IsAuthenticated || _currentUserService.UserId is null)
        {
            throw new UnauthorizedException("Authentication is required.");
        }

        return _currentUserService.UserId.Value;
    }
}
