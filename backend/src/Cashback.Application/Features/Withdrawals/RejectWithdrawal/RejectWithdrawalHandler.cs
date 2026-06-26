using Cashback.Application.Features.Withdrawals.Common;
using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Domain.Enums;
using Cashback.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cashback.Application.Features.Withdrawals.RejectWithdrawal;

/// <summary>
/// Handles rejection of a pending withdrawal request by an administrator.
/// </summary>
public sealed class RejectWithdrawalHandler
    : IRequestHandler<RejectWithdrawalCommand, WithdrawalActionResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IWithdrawalRepository _withdrawalRepository;
    private readonly IBalanceService _balanceService;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<RejectWithdrawalHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the reject withdrawal handler.
    /// </summary>
    public RejectWithdrawalHandler(
        ICurrentUserService currentUserService,
        IWithdrawalRepository withdrawalRepository,
        IBalanceService balanceService,
        IAuditLogService auditLogService,
        ILogger<RejectWithdrawalHandler> logger)
    {
        _currentUserService = currentUserService;
        _withdrawalRepository = withdrawalRepository;
        _balanceService = balanceService;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<WithdrawalActionResponse> Handle(
        RejectWithdrawalCommand request,
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

        if (withdrawal.Status != WithdrawalStatus.Pending)
        {
            throw new BusinessRuleException("Only pending withdrawals can be rejected.");
        }

        var previousStatus = withdrawal.Status;
        var balanceBefore = await _balanceService.GetOperationalAvailableBalanceAsync(
            withdrawal.UserId,
            cancellationToken);

        withdrawal.Reject();

        var balanceAfter = balanceBefore + withdrawal.Amount;

        var transaction = Transaction.Create(
            withdrawal.UserId,
            TransactionType.WithdrawalRejected,
            amount: withdrawal.Amount,
            balanceBefore: balanceBefore,
            balanceAfter: balanceAfter,
            referenceId: withdrawal.Id,
            description: "Withdrawal rejected");

        await _withdrawalRepository.ProcessWithdrawalUpdateAsync(
            withdrawal,
            transaction,
            cancellationToken);

        await _auditLogService.LogWithdrawalActionAsync(
            adminUserId,
            withdrawal.Id,
            AuditAction.WithdrawalRejected,
            previousStatus,
            withdrawal.Status,
            request.Reason?.Trim(),
            cancellationToken);

        _logger.LogInformation(
            "Withdrawal {WithdrawalId} rejected by admin {AdminUserId}",
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
