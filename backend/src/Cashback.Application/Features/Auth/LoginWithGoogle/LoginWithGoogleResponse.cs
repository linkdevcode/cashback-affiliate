using Cashback.Domain.Enums;

namespace Cashback.Application.Features.Auth.LoginWithGoogle;

/// <summary>
/// Response returned after successful Google authentication.
/// </summary>
public sealed class LoginWithGoogleResponse
{
    /// <summary>
    /// JWT access token.
    /// </summary>
    public string AccessToken { get; init; } = null!;

    /// <summary>
    /// Opaque refresh token for session renewal.
    /// </summary>
    public string RefreshToken { get; init; } = null!;

    /// <summary>
    /// Authenticated user profile summary.
    /// </summary>
    public UserInfoDto User { get; init; } = null!;
}

/// <summary>
/// Authenticated user profile summary.
/// </summary>
public sealed class UserInfoDto
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
}
