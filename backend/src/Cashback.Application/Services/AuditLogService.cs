using System.Text.Json;
using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Domain.Enums;

namespace Cashback.Application.Services;

/// <summary>
/// Persists business audit events to the database.
/// </summary>
public sealed class AuditLogService : IAuditLogService
{
    private readonly IAuditLogRepository _auditLogRepository;

    /// <summary>
    /// Initializes a new instance of the audit log service.
    /// </summary>
    public AuditLogService(IAuditLogRepository auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
    }

    /// <inheritdoc/>
    public async Task LogOrderSynchronizationAsync(
        Guid userId,
        Guid orderId,
        AuditAction action,
        OrderStatus? previousStatus,
        OrderStatus newStatus,
        Guid webhookEventId,
        CancellationToken cancellationToken)
    {
        var metadata = JsonSerializer.Serialize(new
        {
            previousStatus = previousStatus?.ToString(),
            newStatus = newStatus.ToString(),
            webhookEventId
        });

        var auditLog = AuditLog.Create(
            userId,
            action,
            "Order",
            orderId,
            metadata);

        await _auditLogRepository.AddAsync(auditLog, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task LogWithdrawalActionAsync(
        Guid adminUserId,
        Guid withdrawalId,
        AuditAction action,
        WithdrawalStatus previousStatus,
        WithdrawalStatus newStatus,
        string? reason,
        CancellationToken cancellationToken)
    {
        var metadata = JsonSerializer.Serialize(new
        {
            previousStatus = previousStatus.ToString(),
            newStatus = newStatus.ToString(),
            reason
        });

        var auditLog = AuditLog.Create(
            adminUserId,
            action,
            "Withdrawal",
            withdrawalId,
            metadata);

        await _auditLogRepository.AddAsync(auditLog, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task LogUserStatusChangeAsync(
        Guid adminUserId,
        Guid targetUserId,
        AuditAction action,
        UserStatus previousStatus,
        UserStatus newStatus,
        CancellationToken cancellationToken)
    {
        var metadata = JsonSerializer.Serialize(new
        {
            previousStatus = previousStatus.ToString(),
            newStatus = newStatus.ToString()
        });

        var auditLog = AuditLog.Create(
            adminUserId,
            action,
            "User",
            targetUserId,
            metadata);

        await _auditLogRepository.AddAsync(auditLog, cancellationToken);
    }
}
