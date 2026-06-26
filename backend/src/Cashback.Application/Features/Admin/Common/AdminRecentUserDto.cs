using Cashback.Domain.Enums;

namespace Cashback.Application.Features.Admin.Common;

/// <summary>
/// Recent user summary for admin dashboard activity widgets.
/// </summary>
public sealed class AdminRecentUserDto
{
    /// <summary>
    /// User identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// User email address.
    /// </summary>
    public string Email { get; init; } = null!;

    /// <summary>
    /// User display name.
    /// </summary>
    public string FullName { get; init; } = null!;

    /// <summary>
    /// Current user status value.
    /// </summary>
    public UserStatus Status { get; init; }

    /// <summary>
    /// Human-readable user status name.
    /// </summary>
    public string StatusName { get; init; } = null!;

    /// <summary>
    /// UTC timestamp when the user account was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
