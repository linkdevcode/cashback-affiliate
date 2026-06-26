using Cashback.Application.Features.Withdrawals.Common;
using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Cashback.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cashback.Application.Features.Withdrawals.CompleteWithdrawal;

/// <summary>
/// Handles completion of an approved withdrawal request by an administrator.
/// </summary>
public sealed class CompleteWithdrawalHandler
    : IRequestHandler<CompleteWithdrawalCommand, WithdrawalActionResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IWithdrawalRepository _withdrawalRepository;
    private readonly IBalanceService _balanceService;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<CompleteWithdrawalHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the complete withdrawal handler.
    /// </summary>
    public CompleteWithdrawalHandler(
        ICurrentUserService currentUserService,
        IWithdrawalRepository withdrawalRepository,
        IBalanceService balanceService,
        IAuditLogService auditLogService,
        ILogger<CompleteWithdrawalHandler> logger)
    {
        _currentUserService = currentUserService;
        _withdrawalRepository = withdrawalRepository;
        _balanceService = balanceService;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<WithdrawalActionResponse> Handle(
        CompleteWithdrawalCommand request,
        CancellationToken cancellationToken)
    {
        var adminUserId = WithdrawalAdminAuthorization.RequireAdminUserId(_currentUserService);

        var withdrawal = await _withdrawalRepository.GetByIdForUpdateAsync(
            request.WithdrawalId,
            cancellationToken);

        if (withdrawal is null)
        {
            throw new NotFoundException("Withdrawal not found.");
        }

        if (withdrawal.Status != WithdrawalStatus.Approved)
        {
            throw new BusinessRuleException("Only approved withdrawals can be completed.");
        }

        var previousStatus = withdrawal.Status;
        var balanceBefore = await _balanceService.GetOperationalAvailableBalanceAsync(
            withdrawal.UserId,
            cancellationToken);

        withdrawal.Complete();

        var transaction = Transaction.Create(
            withdrawal.UserId,
            TransactionType.WithdrawalCompleted,
            amount: 0m,
            balanceBefore: balanceBefore,
            balanceAfter: balanceBefore,
            referenceId: withdrawal.Id,
            description: "Withdrawal completed");

        await _withdrawalRepository.ProcessWithdrawalUpdateAsync(
            withdrawal,
            transaction,
            cancellationToken);

        await _auditLogService.LogWithdrawalActionAsync(
            adminUserId,
            withdrawal.Id,
            AuditAction.WithdrawalCompleted,
            previousStatus,
            withdrawal.Status,
            reason: null,
            cancellationToken);

        _logger.LogInformation(
            "Withdrawal {WithdrawalId} completed by admin {AdminUserId}",
            withdrawal.Id,
            adminUserId);

        return ToResponse(withdrawal);
    }

    /// <summary>
    /// Maps a withdrawal entity to an action response.
    /// </summary>
    private static WithdrawalActionResponse ToResponse(Withdrawal withdrawal)
    {
        return new WithdrawalActionResponse
        {
            Id = withdrawal.Id,
            Status = (int)withdrawal.Status,
            StatusName = withdrawal.Status.ToString()
        };
    }
}
