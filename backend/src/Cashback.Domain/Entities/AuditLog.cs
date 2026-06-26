using Cashback.Domain.Common;
using Cashback.Domain.Enums;

namespace Cashback.Domain.Entities;

/// <summary>
/// Immutable audit record for system and business events.
/// </summary>
public class AuditLog : BaseEntity
{
    /// <summary>
    /// User associated with the audited action.
    /// </summary>
    public Guid? UserId { get; private set; }

    /// <summary>
    /// Action performed on the entity.
    /// </summary>
    public AuditAction Action { get; private set; }

    /// <summary>
    /// Name of the affected entity type.
    /// </summary>
    public string EntityName { get; private set; } = null!;

    /// <summary>
    /// Identifier of the affected entity.
    /// </summary>
    public Guid? EntityId { get; private set; }

    /// <summary>
    /// Additional JSON metadata for the audit event.
    /// </summary>
    public string? Metadata { get; private set; }

    /// <summary>
    /// UTC timestamp when the audit event was recorded.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Required by Entity Framework.
    /// </summary>
    private AuditLog()
    {
    }

    /// <summary>
    /// Creates a new audit log entry.
    /// </summary>
    public static AuditLog Create(
        Guid? userId,
        AuditAction action,
        string entityName,
        Guid? entityId,
        string? metadata)
    {
        return new AuditLog
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            Metadata = metadata,
            CreatedAt = DateTime.UtcNow
        };
    }
}
