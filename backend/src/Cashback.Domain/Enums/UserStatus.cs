namespace Cashback.Domain.Enums;

/// <summary>
/// Platform user account status.
/// </summary>
public enum UserStatus
{
    /// <summary>
    /// Account is active and can use the platform.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Account is suspended and cannot access platform features.
    /// </summary>
    Suspended = 2,

    /// <summary>
    /// Account is marked as deleted.
    /// </summary>
    Deleted = 3
}
