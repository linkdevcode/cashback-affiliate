namespace Cashback.Domain.Common;

/// <summary>
/// Base type for entities with creation and update timestamps.
/// </summary>
public abstract class AuditableEntity : BaseEntity
{
    /// <summary>
    /// UTC timestamp when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// UTC timestamp when the entity was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; protected set; }
}
