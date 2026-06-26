using Cashback.Domain.Entities;

namespace Cashback.Application.Interfaces;

/// <summary>
/// Persistence abstraction for audit logs.
/// </summary>
public interface IAuditLogRepository
{
    /// <summary>
    /// Persists a new audit log entry.
    /// </summary>
    Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken);
}
