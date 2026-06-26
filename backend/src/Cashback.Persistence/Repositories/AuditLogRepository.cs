using Cashback.Application.Interfaces;
using Cashback.Domain.Entities;
using Cashback.Persistence.Context;

namespace Cashback.Persistence.Repositories;

/// <summary>
/// Entity Framework implementation of audit log persistence.
/// </summary>
public class AuditLogRepository : IAuditLogRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the audit log repository.
    /// </summary>
    public AuditLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken)
    {
        await _context.AuditLogs.AddAsync(auditLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
