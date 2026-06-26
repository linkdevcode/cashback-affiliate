using Cashback.Domain.Enums;

namespace Cashback.Application.Features.Admin.Users.Common;

/// <summary>
/// User summary for admin user list responses.
/// </summary>
public sealed class AdminUserDto
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
    /// User role value.
    /// </summary>
    public UserRole Role { get; init; }

    /// <summary>
    /// User account status value.
    /// </summary>
    public UserStatus Status { get; init; }

    /// <summary>
    /// UTC timestamp when the account was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
