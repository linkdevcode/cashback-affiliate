using Cashback.Domain.Common;

namespace Cashback.Domain.Entities;

public class SystemSetting : BaseEntity
{
    public string Key { get; private set; } = null!;

    public string Value { get; private set; } = null!;

    public string? Description { get; private set; }

    public string Category { get; private set; } = null!;

    public DateTime UpdatedAt { get; private set; }

    public Guid? UpdatedBy { get; private set; }
}
