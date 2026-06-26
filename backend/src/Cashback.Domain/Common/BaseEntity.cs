namespace Cashback.Domain.Common;

/// <summary>
/// Base type for all domain entities.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Unique entity identifier.
    /// </summary>
    public Guid Id { get; protected set; }
}
