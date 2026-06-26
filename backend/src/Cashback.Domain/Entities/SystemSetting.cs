using Cashback.Domain.Common;

namespace Cashback.Domain.Entities;

/// <summary>
/// Configurable system setting stored in the database.
/// </summary>
public class SystemSetting : BaseEntity
{
    /// <summary>
    /// Unique setting key.
    /// </summary>
    public string Key { get; private set; } = null!;

    /// <summary>
    /// Current setting value.
    /// </summary>
    public string Value { get; private set; } = null!;

    /// <summary>
    /// Human-readable description of the setting.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Category used to group related settings.
    /// </summary>
    public string Category { get; private set; } = null!;

    /// <summary>
    /// UTC timestamp when the setting was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Identifier of the user who last updated the setting.
    /// </summary>
    public Guid? UpdatedBy { get; private set; }
}
