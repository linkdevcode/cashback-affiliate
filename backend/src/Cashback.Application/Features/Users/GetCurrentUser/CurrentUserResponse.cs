using Cashback.Domain.Enums;

namespace Cashback.Application.Features.Users.GetCurrentUser;

/// <summary>
/// Current authenticated user profile response.
/// </summary>
public sealed class CurrentUserResponse
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
}
