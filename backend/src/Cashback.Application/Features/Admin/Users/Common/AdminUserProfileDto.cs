using Cashback.Domain.Enums;

namespace Cashback.Application.Features.Admin.Users.Common;

/// <summary>
/// User profile information for admin user detail responses.
/// </summary>
public sealed class AdminUserProfileDto
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
    /// URL of the user avatar image.
    /// </summary>
    public string? AvatarUrl { get; init; }

    /// <summary>
    /// User role value.
    /// </summary>
    public UserRole Role { get; init; }

    /// <summary>
    /// User account status value.
    /// </summary>
    public UserStatus Status { get; init; }

    /// <summary>
    /// Balance available for withdrawal.
    /// </summary>
    public decimal AvailableBalance { get; init; }

    /// <summary>
    /// Cashback amount awaiting approval.
    /// </summary>
    public decimal PendingBalance { get; init; }

    /// <summary>
    /// Total approved cashback earned by the user.
    /// </summary>
    public decimal LifetimeCashback { get; init; }

    /// <summary>
    /// UTC timestamp of the most recent login.
    /// </summary>
    public DateTime? LastLoginAt { get; init; }

    /// <summary>
    /// UTC timestamp when the account was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
