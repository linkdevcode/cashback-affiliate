using Cashback.Domain.Enums;

namespace Cashback.Application.Interfaces;

/// <summary>
/// Records business audit events.
/// </summary>
public interface IAuditLogService
{
    /// <summary>
    /// Records an order synchronization audit event.
    /// </summary>
    Task LogOrderSynchronizationAsync(
        Guid userId,
        Guid orderId,
        AuditAction action,
        OrderStatus? previousStatus,
        OrderStatus newStatus,
        Guid webhookEventId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Records a withdrawal processing audit event.
    /// </summary>
    Task LogWithdrawalActionAsync(
        Guid adminUserId,
        Guid withdrawalId,
        AuditAction action,
        WithdrawalStatus previousStatus,
        WithdrawalStatus newStatus,
        string? reason,
        CancellationToken cancellationToken);

    /// <summary>
    /// Records a user status change audit event.
    /// </summary>
    Task LogUserStatusChangeAsync(
        Guid adminUserId,
        Guid targetUserId,
        AuditAction action,
        UserStatus previousStatus,
        UserStatus newStatus,
        CancellationToken cancellationToken);
}
